using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POSWPF.View.Utilities;
using POSWPF.View.ViewModels;
using System.Windows;

namespace POSWPF.View {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public App() {
            Host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .ConfigureServices(ServicesConfigurationManager.ConfigureServices)
                .Build();
        }

        public static IHost Host { get; private set; } = null!;

        protected override async void OnStartup(StartupEventArgs e) {

            await Host.StartAsync();

            var mainWindow = new MainWindow() { DataContext = Host.Services.GetRequiredService<MainViewModel>() };
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e) {
            await Host.StopAsync();

            base.OnExit(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e) {

        }
    }

}
