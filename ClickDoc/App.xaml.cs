using ClickDoc.Models;
using ClickDoc.Utils;
using ClickDoc.ViewModels;
using ClickDoc.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ClickDoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowVM>();

            services.AddTransient<AcceptanceTransferActVM>();
            services.AddTransient<ActAcceptanceTransferWindow>();

            services.AddTransient<AcceptanceTransferActContractData>();
            services.AddTransient<FormData>();

            services.AddSingleton<INavigationService, NavigationService>();
        }
    }

}
