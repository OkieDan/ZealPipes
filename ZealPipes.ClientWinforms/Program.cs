using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using ZealPipes.ClientWinforms.Views;
using ZealPipes.Common;
using ZealPipes.Services.Helpers;
using ZealPipes.Services;

namespace ZealPipes.ClientWinforms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();

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
            //while (ShowMenu(zealMessageService)) ;
            #endregion

            zealMessageService.StopProcessing();




            Application.Run(new MainFormView());
        }
    }
}