using ClickDoc.Database;
using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Generators;
using ClickDoc.Models;
using ClickDoc.Utils;
using ClickDoc.ViewModels;
using ClickDoc.ViewModels.Contractors;
using ClickDoc.ViewModels.Contracts;
using ClickDoc.ViewModels.Entrepreneurs;
using ClickDoc.Views;
using ClickDoc.Views.Contractors;
using ClickDoc.Views.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ClickDoc
{
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainMenuWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<MainMenuWindow>();
            services.AddTransient<MainMenuVM>();

            services.AddTransient<AcceptanceTransferActVM>();
            services.AddTransient<ActAcceptanceTransferWindow>();

            services.AddTransient<AcceptanceTransferActContractData>();
            services.AddTransient<FormData>();

            services.AddTransient<EntrepreneursWindow>();
            services.AddTransient<EntrepreneursVM>();

            services.AddTransient<NewEntrepreneurWindow>();
            services.AddTransient<NewEntrepreneurVM>();

            services.AddTransient<ContractorsWindow>();
            services.AddTransient<ContractorsVM>();

            services.AddTransient<NewContractorWindow>();
            services.AddTransient<NewContractorVM>();

            services.AddTransient<ContractsWindow>();
            services.AddTransient<ContractsVM>();

            services.AddTransient<NewContractWindow>();
            services.AddTransient<NewContractVM>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddDbContext<MyDbContext>();

            services.AddSingleton<IRepository<EntrepreneurEntity>, EntrepreneursRepository>();
            services.AddSingleton<IRepository<ContractorEntity>, ContractorsRepository>();
            services.AddSingleton<IRepository<ContractEntity>, ContractsRepository>();

            services.AddTransient<INotificationService, NotificationService>();

            services.AddTransient<IGeneratorFactory, GeneratorFactory>();
        }
    }

}
