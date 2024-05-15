using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
using System.Collections.Generic;
using System.Linq;
namespace ZealPipes.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build configuration
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            IServiceCollection services = new ServiceCollection();

            // DI
            services.AddSingleton(config);
            services.AddSingleton<ZealSettings>();
            services.AddSingleton<ProcessMonitor>();
            services.AddSingleton<ZealPipeReader>();
            services.AddSingleton<ZealMessageService>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Process messages
            var zealMessageService = serviceProvider.GetService<ZealMessageService>();
            
            // Subscribe to log messages
            //zealMessageService.OnLogMessageReceived += ZealMessageService_OnLogTextReceived;   // Requires "/log on" in game
            // Subscribe to character updates (LogData and Gauge data should be fully populated)
            //zealMessageService.OnCharacterUpdated += MyService_OnCharacterUpdated;

            // Uncommenting these would give you an update for every label and gauge value, but you will receive ~100x as many events compared to 'OnCharacterUpdated' event.
            //zealMessageService.OnLabelMessageReceived += MyService_OnLabelMessageReceived;
           
            zealMessageService.OnGaugeMessageReceived += ZealMessageService_OnGaugeMessageReceived;
            //zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;

            zealMessageService.StartProcessing();
            Console.ReadLine(); // Keep the application running
            zealMessageService.StopProcessing();
        }
        private static void ZealMessageService_OnPlayerMessageReceived(object sender, ZealMessageService.PlayerMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Data.ZoneId}");
        }

        private static void ZealMessageService_OnCharacterUpdated(object sender, Character.CharacterUpdatedEventArgs e)
        {
            Console.WriteLine($"Character.Name '{e.Character.Name}' on Character.ProcessId {e.ProcessId} has an update.");
            Console.WriteLine($"Character.Detail.GaugeData serialized: {JsonSerializer.Serialize(e.Character.Detail.GaugeData)}");
            Console.WriteLine($"Character.Detail.LabelData serialized: {JsonSerializer.Serialize(e.Character.Detail.LabelData)}");
        }
        class Gauge
        {
            public int Value { get; set; }
            public int MaxValue { get; set; }
            public int Length { get; set; }
            public int TopPosition { get; set; }
        }


        private static Dictionary<int, Gauge> gauges = new Dictionary<int, Gauge>();

        private static void DrawGauge(int id, int value, int maxValue, int length)
        {
            if (gauges.ContainsKey(id))
            {
                Gauge gauge = gauges[id];
                Console.SetCursorPosition(0, gauge.TopPosition);
                int filledLength = (int)Math.Round((double)value / maxValue * length);
                int emptyLength = length - filledLength;
                Console.Write("[");
                for (int i = 0; i < filledLength; i++)
                {
                    Console.Write("█"); // Filled block character
                }
                for (int i = 0; i < emptyLength; i++)
                {
                    Console.Write(" "); // Empty block character
                }
                Console.Write($"] {value}/{maxValue}");

                gauge.Value = value;
                gauge.MaxValue = maxValue;
                gauge.Length = length;
            }
            else
            {
                int topPosition = gauges.Count == 0 ? Console.CursorTop : gauges.Last().Value.TopPosition + 1;
                Gauge newGauge = new Gauge
                {
                    Value = value,
                    MaxValue = maxValue,
                    Length = length,
                    TopPosition = topPosition
                };

                int filledLength = (int)Math.Round((double)value / maxValue * length);
                int emptyLength = length - filledLength;
                Console.SetCursorPosition(0, topPosition);
                Console.Write("[");
                for (int i = 0; i < filledLength; i++)
                {
                    Console.Write("█"); // Filled block character
                }
                for (int i = 0; i < emptyLength; i++)
                {
                    Console.Write(" "); // Empty block character
                }
                Console.Write($"] {value}/{maxValue}");

                gauges.Add(id, newGauge);
            }
        }
        private static void ZealMessageService_OnGaugeMessageReceived(object sender, ZealMessageService.GaugeMessageReceivedEventArgs e)
        {
                //  Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Data.Type}: {e.Message.Data.Value}");
                DrawGauge((int)e.Message.Data.Type, e.Message.Data.Value, 1000, 60);
                Console.Write(" ");
                Console.Write(e.Message.Data.Type.ToString());
                Console.Write(" ");
                Console.Write(e.Message.Data.Text);
                Console.Write("                                                                             ");
        }
        private static void ZealMessageService_OnLabelMessageReceived(object sender, ZealMessageService.LabelMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Data.Type}: {e.Message.Data.Value}");
        }

        private static void ZealMessageService_OnLogTextReceived(object sender, ZealMessageService.LogMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Value}");

        }

    }
}


