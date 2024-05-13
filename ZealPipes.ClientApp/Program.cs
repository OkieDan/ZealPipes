using System;
using System.IO.Pipes;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using System.Diagnostics;
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

            // Add services
            services.AddSingleton<ZealSettings>(new ZealSettings(config));
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ZealSettings>();
            services.AddSingleton<ProcessMonitor>();
            services.AddSingleton<ZealMessageService>();
            services.AddSingleton<ZealPipeReader>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Process messages
            var myService = serviceProvider.GetService<ZealMessageService>();
            myService.OnLogMessageReceived += MyService_OnLogTextReceived;
            myService.OnLabelMessageReceived += MyService_OnLabelMessageReceived;
            myService.OnGaugeMessageReceived += MyService_OnGaugeMessageReceived;
            myService.StartProcessing();
            Console.ReadLine(); // Keep the application running
            myService.StopProcessing();
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


