using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Text.Json;
using ZealPipes.Common;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;

#region What all programs should use

#endregion
#pragma warning disable CA1416
partial class Program
{
    static void Main(string[] args)
    {
        // Build configuration
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        IServiceCollection services = new ServiceCollection();

        using (var audioFile = new AudioFileReader("tick.mp3"))

            // Add ZealSettings, ProcessMonitor, ZealPipeReader, ZealMessageService to DI
            services.AddSingleton(config);
        services.AddSingleton<ZealSettings>();                                  // Use to get Zeal settings from appsettings.json (otherwise comment & use next line)
                                                                                //services.AddSingleton(new ZealSettings("eqgame", "zeal", 32768));     // pass settings w/o appsettings.json here
        services.AddSingleton<ProcessMonitor>();
        services.AddSingleton<ZealPipeReader>();
        services.AddSingleton<ZealMessageService>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var zealMessageService = serviceProvider.GetService<ZealMessageService>();
        zealMessageService!.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;
        zealMessageService.StartProcessing();

        Console.Clear();
        while (ShowMenu(zealMessageService))
            System.Threading.Thread.Sleep(5);

        zealMessageService.StopProcessing();
    }

    private static object lockObject = new object();
    private static decimal _mana;
    private static double _delay = 0;
    private static ConcurrentDictionary<string, DateTime> characterUpdates = new ConcurrentDictionary<string, DateTime>();
    private static string selectedCharacterName = null;
    private static int previousCharacterCount = 0;
    private static readonly object syncLock = new object();

    private static void MenuHeader()
    {
        Console.Clear();
        lock (syncLock)
        {
            if (selectedCharacterName == null) {
                Console.Clear();
                Console.WriteLine("Select a character:");
                var characterNames = characterUpdates.Keys.OrderBy(name => name).ToList();
                for (int i = 0; i < characterNames.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {characterNames[i]}{(characterNames[i] == selectedCharacterName ? "*" : "")}");
                }
                Console.WriteLine("X: Exit");
            }
            else
            {

            }
        }

        Console.Write($"  Current Delay {_delay}ms.");
        Console.Write($"  +:Increase Delay");
        Console.Write($"  -:Decrease Delay");
        Console.Write("   X:Exit\n");
    }

    private static bool ShowMenu(ZealMessageService zealMessageService)
    {
        MenuHeader();
        var key = Console.ReadKey(false).Key;
        if (key == ConsoleKey.X)
            return false;

        var characterNames = characterUpdates.Keys.OrderBy(name => name).ToList();
        if (key == ConsoleKey.Add || key == ConsoleKey.OemPlus)
        {
            _delay = Math.Min(6000, _delay + 250);
        }
        else if (key == ConsoleKey.Subtract || key == ConsoleKey.OemMinus)
        {
            _delay = Math.Max(0, _delay - 250);
        }
        else if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
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


    private static void ZealMessageService_OnCharacterUpdated(object? sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
    {
        var now = DateTime.Now;
        if (e.Character == null || e.Character.Detail.PlayerData == null || string.IsNullOrEmpty(e.Character.Name))
            return;

        characterUpdates[e.Character.Name] = now;

        // Remove characters that haven't been updated in the last 2 seconds
        var charactersToRemove = characterUpdates
            .Where(kvp => (now - kvp.Value).TotalSeconds > 2)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var character in charactersToRemove)
            characterUpdates.TryRemove(character, out _);

        // Check if the number of characters has changed
        if (characterUpdates.Count != previousCharacterCount)
        {
            Interlocked.Exchange(ref previousCharacterCount, characterUpdates.Count);
            MenuHeader();
        }

        Task.Run(() => ProcessCharacterUpdateAsync(e.Character));

    }

    private static void ProcessCharacterUpdateAsync(ZealCharacter character)
    {
        var now = DateTime.Now;
        if (selectedCharacterName == character.Name)
        {

            decimal mana = character.Detail.GaugeData.Where(x => (ZealPipes.Common.GaugeType)x.Type == GaugeType.Mana).FirstOrDefault()?.Value ?? _mana;
            if (mana < _mana)
                _mana = mana;
            else if (mana > _mana && mana == 1000)
            {
                _mana = mana;
                // Speak "Full Mana"
                using (var synth = new SpeechSynthesizer())
                {
                    synth.Rate = 4;
                    synth.SelectVoice("Microsoft Zira Desktop");
                    synth.Speak($"Full Mana");
                }
            }
            else if (mana > _mana)
            {
                //Debug.WriteLine($"Tick {mana}");
                lock (lockObject)
                {
                    _mana = mana;
                    //Debug.WriteLine(JsonSerializer.Serialize(e));
                    Task.Run(() =>
                    {
                        Thread.Sleep((int)_delay);

                        AudioFileReader _audioFile = new AudioFileReader("tick.mp3");
                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(_audioFile);
                            outputDevice.Play();
                            while (outputDevice.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    });
                }
                //Debug.WriteLine("Tock");
            }
        }
    }
}
