using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
using ZealPipes.Common;
using ChainMayhem.Extensions;
using ChainMayhem.Models;
using System.Speech.Synthesis;
using System.Collections.Concurrent;

namespace ChainMayhem;

partial class Program
{
    private static ConcurrentDictionary<string, DateTime> characterUpdates = new();
    private static string? selectedCharacterName = null;
    private static int previousCharacterCount = 0;
    private static SpeechSynthesizer synth = null!;
    private static readonly object syncLock = new();
    private static Group? previousGroup;

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

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var zealMessageService = serviceProvider.GetService<ZealMessageService>();

        synth = new SpeechSynthesizer { Rate = 8 };
        synth.SelectVoice("Microsoft Zira Desktop");

        zealMessageService!.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;
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
            Console.WriteLine("\nX: Exit");
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
                selectedCharacterName = characterNames[index];
            }
        }
        else if (key >= ConsoleKey.NumPad1 && key <= ConsoleKey.NumPad9)
        {
            int index = (int)key - (int)ConsoleKey.NumPad1;
            if (index < characterNames.Count)
            {
                selectedCharacterName = characterNames[index];
            }
        }
        return true;
    }

    private static void ZealMessageService_OnCharacterUpdated(object sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
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

        Task.Run(() => ProcessCharacterUpdate(e.Character));
    }

    private static void ProcessCharacterUpdate(ZealCharacter character)
    {
        if (selectedCharacterName == character.Name)
        {
            var group = character.ToGroup();
            CheckPetHealthOrCharmBreak(group);
            previousGroup = group;
        }
    }

    private static void CheckPetHealthOrCharmBreak(Group currentGroup)
    {
        if (previousGroup == null) return;

        foreach (var currentMember in currentGroup.Members)
        {
            var previousMember = previousGroup.Members.FirstOrDefault(m => m.Name == currentMember.Name);
            if (previousMember == null) continue;

            if (currentMember.PetHp == 0 && previousMember.PetHp > 10)
            {
                synth.SpeakAsync($"Charm Break {currentMember.Name}");
            }
            else if (currentMember.PetHp.Between(1, 19) && previousMember.PetHp > 19)
            {
                synth.SpeakAsync("Heal Pet");
            }
        }
    }
}
