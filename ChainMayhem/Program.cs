using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
using ZealPipes.Common;
using ChainMayhem.Extensions;
using ChainMayhem.Models;
using ChainMayhem.Services;
using System.Speech.Synthesis;
using System.Collections.Concurrent;

namespace ChainMayhem;

partial class Program
{
    private static ConcurrentDictionary<string, DateTime> characterUpdates = new();
    private static readonly ConcurrentDictionary<int, GroupMessage.GroupData[]> groupMembersByProcess = new();
    private static string? selectedCharacterName = null;
    private static int previousCharacterCount = 0;
    private static SpeechSynthesizer synth = null!;
    private static int speechRate = 8;
    private static readonly object syncLock = new();
    private static Group? previousGroup;
    private static HashSet<string> ignoredPlayers = new(StringComparer.OrdinalIgnoreCase);
    private static SettingsManager settingsManager = null!;

    static void Main(string[] args)
    {
#if DEBUG
        var environment = "Development";
#else
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
#endif

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();

        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(config);
        services.AddSingleton<ZealSettings>();
        services.AddSingleton<ProcessMonitor>();
        services.AddSingleton<ZealPipeReader>();
        services.AddSingleton<ZealMessageService>();
        services.AddSingleton<SettingsManager>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var zealMessageService = serviceProvider.GetService<ZealMessageService>();
        settingsManager = serviceProvider.GetService<SettingsManager>()!;

        // Load persisted settings
        selectedCharacterName = settingsManager.Settings.SelectedCharacterName;
        speechRate = settingsManager.Settings.SpeechRate;
        ignoredPlayers = settingsManager.Settings.IgnoredPlayers;

        synth = new SpeechSynthesizer { Rate = speechRate };
        synth.SelectVoice("Microsoft Zira Desktop");

        zealMessageService!.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;
        zealMessageService.OnGroupMessageReceived += (_, e) => groupMembersByProcess[e.ProcessId] = e.Message.Data;
        zealMessageService.OnConnectionTerminated += (sender, e) => groupMembersByProcess.TryRemove(e.ProcessId, out _);
        zealMessageService.StartProcessing();

        try
        {
            Console.Clear();
            Console.WriteLine("=== ChainMayhem - Pet Monitor ===\n");
            while (ShowMenu(zealMessageService)) ;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            zealMessageService.StopProcessing();
            synth.Dispose();
        }
    }

    private static void MenuHeader()
    {
        lock (syncLock)
        {
            Console.Clear();
            Console.WriteLine("=== ChainMayhem - Pet Monitor ===");
            Console.WriteLine("\nSelect a character:");
            var characterNames = characterUpdates.Keys.OrderBy(name => name).ToList();
            for (int i = 0; i < characterNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {characterNames[i]}{(characterNames[i] == selectedCharacterName ? " *" : "")}");
            }
            Console.WriteLine($"\nSpeech Speed: {speechRate} (-10 to 10)");
            Console.WriteLine("+: Increase Speed  -: Decrease Speed");
            Console.WriteLine("I: Ignore Player (pet messages)");
            if (ignoredPlayers.Count > 0)
            {
                Console.WriteLine($"   Ignored: {string.Join(", ", ignoredPlayers)}");
            }
            Console.WriteLine($"C: Track pet-break for Enchanter, Druid and Necro only (requires /pipeverbose on): {(settingsManager.Settings.TrackCharmBreaksForCharmClassesOnly ? "On" : "Off")}");
            Console.WriteLine("X: Exit");
        }
    }

    private static bool ShowMenu(ZealMessageService zealMessageService)
    {
        MenuHeader();
        var key = Console.ReadKey(false).Key;
        if (key == ConsoleKey.X)
            return false;

        var characterNames = characterUpdates.Keys.OrderBy(name => name).ToList();
        if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
        {
            int index = (int)key - (int)ConsoleKey.D1;
            if (index < characterNames.Count)
            {
                SelectCharacter(characterNames[index]);
            }
        }
        else if (key >= ConsoleKey.NumPad1 && key <= ConsoleKey.NumPad9)
        {
            int index = (int)key - (int)ConsoleKey.NumPad1;
            if (index < characterNames.Count)
            {
                SelectCharacter(characterNames[index]);
            }
        }
        else if (key == ConsoleKey.Add || key == ConsoleKey.OemPlus)
        {
            if (speechRate < 10)
            {
                speechRate++;
                synth.Rate = speechRate;
                settingsManager.UpdateSpeechRate(speechRate);
            }
        }
        else if (key == ConsoleKey.Subtract || key == ConsoleKey.OemMinus)
        {
            if (speechRate > -10)
            {
                speechRate--;
                synth.Rate = speechRate;
                settingsManager.UpdateSpeechRate(speechRate);
            }
        }
        else if (key == ConsoleKey.I)
        {
            AddIgnoredPlayer();
        }
        else if (key == ConsoleKey.C)
        {
            lock (syncLock)
            {
                settingsManager.UpdateCharmBreakClassFilter(!settingsManager.Settings.TrackCharmBreaksForCharmClassesOnly);
            }
        }
        return true;
    }

    private static void SelectCharacter(string characterName)
    {
        lock (syncLock)
        {
            selectedCharacterName = characterName;
            previousGroup = null;
            settingsManager.UpdateSelectedCharacter(characterName);
        }
    }

    private static void ZealMessageService_OnCharacterUpdated(object? sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
    {
        var now = DateTime.Now;
        if (e.Character == null || e.Character.Detail.PlayerData == null || string.IsNullOrEmpty(e.Character.Name))
            return;

        characterUpdates[e.Character.Name] = now;

        var charactersToRemove = characterUpdates
            .Where(kvp => (now - kvp.Value).TotalSeconds > 2)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var character in charactersToRemove)
            characterUpdates.TryRemove(character, out _);

        if (characterUpdates.Count != previousCharacterCount)
        {
            Interlocked.Exchange(ref previousCharacterCount, characterUpdates.Count);
            MenuHeader();
        }

        ProcessCharacterUpdate(e.Character);
    }

    private static void ProcessCharacterUpdate(ZealCharacter character)
    {
        lock (syncLock)
        {
            if (selectedCharacterName == character.Name)
            {
                groupMembersByProcess.TryGetValue(character.ProcessId, out var members);
                var group = character.ToGroup(members);
                foreach (var alert in PetAlertService.GetAlerts(previousGroup, group,
                    settingsManager.Settings.TrackCharmBreaksForCharmClassesOnly, ignoredPlayers))
                    synth.SpeakAsync(alert);
                previousGroup = group;
            }
        }
    }

    private static void AddIgnoredPlayer()
    {
        lock (syncLock)
        {
            Console.Clear();
            Console.WriteLine("=== Ignore Player (pet messages) ===");
            Console.WriteLine();
            if (ignoredPlayers.Count > 0)
            {
                Console.WriteLine($"Currently ignored: {string.Join(", ", ignoredPlayers)}");
                Console.WriteLine();
            }
            Console.Write("Enter player name: ");
            var playerName = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(playerName))
            {
                // Use SettingsManager to add (it handles normalization and persistence)
                if (settingsManager.AddIgnoredPlayer(playerName))
                {
                    Console.WriteLine($"Added '{playerName}' to ignored players.");
                }
                else
                {
                    Console.WriteLine($"'{playerName}' is already in the ignored list.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
