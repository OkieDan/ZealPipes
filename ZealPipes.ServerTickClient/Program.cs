using Microsoft.Extensions.Configuration;
using NAudio.Wave;
using ZealPipes.Common;
using ZealPipes.Services.Helpers;
using ZealPipes.Services;
using Microsoft.Extensions.DependencyInjection;
using ZealPipes.Common.Models;
using System.Text.Json;
using System.Diagnostics;
using System.Speech.Synthesis;

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
    private static void MenuHeader()
    {
        Console.Clear();
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

        if (key == ConsoleKey.Add || key == ConsoleKey.OemPlus)
        {
            _delay = Math.Min(6000, _delay + 250);
        }
        else if (key == ConsoleKey.Subtract || key == ConsoleKey.OemMinus)
        {
            _delay = Math.Max(0, _delay - 250);
        }
        return true;
    }


    private static void ZealMessageService_OnCharacterUpdated(object? sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
    {
        decimal mana = e.Character.Detail.GaugeData.Where(x => (ZealPipes.Common.GaugeType)x.Type == GaugeType.Mana).FirstOrDefault()?.Value ?? _mana;
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
        else if(mana > _mana)
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
