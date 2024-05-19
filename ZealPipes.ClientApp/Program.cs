using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
using ZealPipes.Common;
namespace ZealPipes.ClientApp
{
    partial class Program
    {
        private static bool UseZealConsoleUi = true;  // set false to be spammed with messages from Zeal

        static void Main(string[] args)
        {
            // Build configuration
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            IServiceCollection services = new ServiceCollection();

            #region What all programs should use
            // Add ZealSettings, ProcessMonitor, ZealPipeReader, ZealMessageService to DI
            services.AddSingleton(config);
            services.AddSingleton<ZealSettings>();                                  // Use to get Zeal settings from appsettings.json (otherwise comment & use next line)
            //services.AddSingleton(new ZealSettings("eqgame", "zeal", 32768));     // pass settings w/o appsettings.json here
            services.AddSingleton<ProcessMonitor>();
            services.AddSingleton<ZealPipeReader>();
            services.AddSingleton<ZealMessageService>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();



            // Process messages
            var zealMessageService = serviceProvider.GetService<ZealMessageService>();

            // Subscribe to log messages
            //zealMessageService.OnLogMessageReceived += ZealMessageService_OnLogMessageReceived;

            // Subscribe to player messages (currently only ZoneId)
            //zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;

            // Subscribe to character updates (Label and Gauge data should be fully populated)
            //zealMessageService.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;

            // Uncommenting these would give you an update for every label and gauge value, but you will receive ~100x as many events compared to 'OnCharacterUpdated' event.
            //zealMessageService.OnLabelMessageReceived += ZealMessageService_OnLabelMessageReceived;
            //zealMessageService.OnGaugeMessageReceived += ZealMessageService_OnGaugeMessageReceived;

            zealMessageService.StartProcessing();

            #endregion

            #region Client Specific
            Console.Clear();
            while (ShowMenu(zealMessageService));
            #endregion

            zealMessageService.StopProcessing();
        }

        #region Client Specific
        private static ConsoleKey _lastMenuOption;
        private static void ClearHandlers(ZealMessageService zealMessageService)
        {
            zealMessageService.OnLogMessageReceived -= ZealMessageService_OnLogMessageReceived;
            zealMessageService.OnLabelMessageReceived -= ZealMessageService_OnLabelMessageReceived;
            zealMessageService.OnGaugeMessageReceived -= ZealMessageService_OnGaugeMessageReceived;
            zealMessageService.OnPlayerMessageReceived -= ZealMessageService_OnPlayerMessageReceived;
            zealMessageService.OnCharacterUpdated -= ZealMessageService_OnCharacterUpdated;
            zealMessageService.OnPipeCmdMessageReceived -= ZealMessageService_OnPipeCmdMessageReceived;
        }

        private static void MenuHeader()
        {
            Console.Clear();
            Console.Write($"{(_lastMenuOption == ConsoleKey.D1 ? ">" : "")}1:Labels");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D2 ? "  >" : "  ")}2:Gauges");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D3 ? "  >" : "  ")}3:Player");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D4 ? "  >" : "  ")}4:Character (Gauges, Labels, Player)");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D5 ? "  >" : "  ")}5:UI");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D6 ? "  >" : "  ")}6:Chat Log");
            Console.Write($"{(_lastMenuOption == ConsoleKey.D7 ? "  >" : "  ")}7:/Pipe");
            Console.Write("   X:Exit\n");
        }
        private static bool ShowMenu(ZealMessageService zealMessageService)
        {
            MenuHeader();
            var key = Console.ReadKey(false).Key;
            if (key == ConsoleKey.X)
                return false;
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
            {
                _lastMenuOption = ConsoleKey.D1;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnLabelMessageReceived += ZealMessageService_OnLabelMessageReceived;
            }
            if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
            {
                _lastMenuOption = ConsoleKey.D2;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnGaugeMessageReceived += ZealMessageService_OnGaugeMessageReceived;
            }
            if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3)
            {
                _lastMenuOption = ConsoleKey.D3;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;
            }
            if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4)
            {
                _lastMenuOption = ConsoleKey.D4;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;
                zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;
            }
            if (key == ConsoleKey.D5 || key == ConsoleKey.NumPad5)
            {
                _lastMenuOption = ConsoleKey.D5;
                UseZealConsoleUi = true;
                ClearHandlers(zealMessageService);
                zealMessageService.OnGaugeMessageReceived += ZealMessageService_OnGaugeMessageReceived;
            }
            if (key == ConsoleKey.D6 || key == ConsoleKey.NumPad6)
            {
                _lastMenuOption = ConsoleKey.D6;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnLogMessageReceived += ZealMessageService_OnLogMessageReceived;
            }
            if (key == ConsoleKey.D7 || key == ConsoleKey.NumPad7)
            {
                _lastMenuOption = ConsoleKey.D7;
                UseZealConsoleUi = false;
                ClearHandlers(zealMessageService);
                zealMessageService.OnPipeCmdMessageReceived += ZealMessageService_OnPipeCmdMessageReceived;
            }

            return true;
        }

        private static void ZealMessageService_OnPipeCmdMessageReceived(object sender, ZealMessageService.PipeCmdMessageReceivedEventArgs e)
        {
            if(_lastMenuOption == ConsoleKey.D7)
            {
                Console.WriteLine($"ZealService(PipeCmd)> {e.Message.Data.Text}");
            }
        }

        private static void ZealMessageService_OnCharacterUpdated(object sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
        {
            if (_lastMenuOption == ConsoleKey.D4)
            {
                MenuHeader();
                Console.WriteLine($"Character.Name '{e.Character.Name}' on Character.ProcessId {e.ProcessId} has an update.");
                Console.WriteLine($"Character.Detail.PlayerData serialized: {JsonSerializer.Serialize(e.Character.Detail.PlayerData)}");
                Console.WriteLine($"Character.Detail.GaugeData serialized: {JsonSerializer.Serialize(e.Character.Detail.GaugeData)}");
                Console.WriteLine($"Character.Detail.LabelData serialized: {JsonSerializer.Serialize(e.Character.Detail.LabelData)}");
            }
        }

        private static void ZealMessageService_OnGaugeMessageReceived(object sender, ZealMessageService.GaugeMessageReceivedEventArgs e)
        {
            if (_lastMenuOption == ConsoleKey.D5)
            {
                foreach (var data in e.Message.Data)
                {
                    ZealConsoleUi.DrawGauge((int)data.Type, Math.Round(data.Value / 10), 100, 60);
                    Console.Write(" ");
                    Console.Write(data.Type.ToString());
                    Console.Write(" ");
                    Console.Write(data.Text);
                    Console.Write("                                                                             ");
                }
            }
            else if (_lastMenuOption == ConsoleKey.D2)
            {
                foreach (var data in e.Message.Data)
                {
                    Console.WriteLine($"ZealService(Gauge)> proc:{e.ProcessId}  char:{e.Message.Character}: {e.Message.Type}: {data.Type}: {data.Value}");
                }
            }
        }

        private static void ZealMessageService_OnPlayerMessageReceived(object sender, ZealMessageService.PlayerMessageReceivedEventArgs e)
        {
            if (_lastMenuOption == ConsoleKey.D3)
            {
                Console.WriteLine($"ZealService(Player)> proc:{e.ProcessId}  char:{e.Message.Character}  zoneId:{e.Message.Data.ZoneId} position:{e.Message.Data.Position.X} {e.Message.Data.Position.Y} {e.Message.Data.Position.Z} heading:{e.Message.Data.heading}");
            }
        }

        private static void ZealMessageService_OnLabelMessageReceived(object sender, ZealMessageService.LabelMessageReceivedEventArgs e)
        {
            if (_lastMenuOption == ConsoleKey.D1)
            {
                foreach (var data in e.Message.Data)
                {
                    Console.WriteLine($"ZealService(Label)> proc:{e.ProcessId}  char:{e.Message.Character}  type:{e.Message.Type}  labelType:{data.Type}  value:{data.Value}");
                }
            }
        }

        private static void ZealMessageService_OnLogMessageReceived(object sender, ZealMessageService.LogMessageReceivedEventArgs e)
        {
            if (!UseZealConsoleUi)
            {
                Console.WriteLine($"ZealService(Log)> proc:{e.ProcessId}  char:{e.Message.Character}  type:{e.Message.Type}  text:{e.Message.Data.Text}");
            }
        }
        #endregion
    }
}
