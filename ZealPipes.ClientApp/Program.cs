using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
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
            zealMessageService.OnLogMessageReceived += MyService_OnLogTextReceived;
            // Subscribe to character updates (LogData and Gauge data should be fully populated)
            //zealMessageService.OnCharacterUpdated += MyService_OnCharacterUpdated;

            // Uncommenting these would give you an update for every label and gauge value, but you will receive ~100x as many events compared to 'OnCharacterUpdated' event.
            //zealMessageService.OnLabelMessageReceived += MyService_OnLabelMessageReceived;
            //zealMessageService.OnGaugeMessageReceived += MyService_OnGaugeMessageReceived;
            zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;

            zealMessageService.StartProcessing();
            Console.ReadLine(); // Keep the application running
            zealMessageService.StopProcessing();
        }
        private static void ZealMessageService_OnPlayerMessageReceived(object sender, ZealMessageService.PlayerMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Data.ZoneId}");
        }

        private static void MyService_OnCharacterUpdated(object sender, Character.CharacterUpdatedEventArgs e)
        {
            Console.WriteLine($"Character.Name '{e.Character.Name}' on Character.ProcessId {e.ProcessId} has an update.");
            Console.WriteLine($"Character.Detail.GaugeData serialized: {JsonSerializer.Serialize(e.Character.Detail.GaugeData)}");
            Console.WriteLine($"Character.Detail.LabelData serialized: {JsonSerializer.Serialize(e.Character.Detail.LabelData)}");
        }

        private static void MyService_OnGaugeMessageReceived(object sender, ZealMessageService.GaugeMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Data.Type}: {e.Message.Data.Value}");
        }

        private static void MyService_OnLabelMessageReceived(object sender, ZealMessageService.LabelMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Data.Type}: {e.Message.Data.Value}");
        }

        private static void MyService_OnLogTextReceived(object sender, ZealMessageService.LogMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message from ZealMessageService {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.Value}");

        }

    }
}


