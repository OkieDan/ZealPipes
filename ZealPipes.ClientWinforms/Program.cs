using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using ZealPipes.ClientWinforms.Views;
using ZealPipes.Common;
using ZealPipes.Services.Helpers;
using ZealPipes.Services;
using ZealPipes.ClientWinforms.Presenters;
using System.Diagnostics;

namespace ZealPipes.ClientWinforms
{
    static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            // Initialize WinForms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Build DI and configuration
            ConfigureServices();

            // Get the MainForm from DI and run it
            var mainForm = ServiceProvider.GetService<MainForm>();
            var svc = ServiceProvider.GetService<ZealMessageService>();
            try
            {
                Application.Run(mainForm);
            }            
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}: {ex.StackTrace}");
                Console.WriteLine($"{ex.Message}: {ex.StackTrace}");
            }
            finally
            {
                svc.StopProcessing();
            }
        }

        private static void ConfigureServices()
        {
            // Build configuration
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            IServiceCollection services = new ServiceCollection();

            // Add ZealSettings, ProcessMonitor, ZealPipeReader, ZealMessageService to DI
            services.AddSingleton(config);
            services.AddSingleton<ZealSettings>();  // Use to get Zeal settings from appsettings.json
            services.AddSingleton<ProcessMonitor>();
            services.AddSingleton<ZealPipeReader>();
            services.AddSingleton<ZealMessageService>();

            // Register MainForm and its presenter
            services.AddSingleton<MainForm>();
            services.AddSingleton<MainPresenter>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}