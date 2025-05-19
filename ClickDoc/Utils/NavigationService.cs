using ClickDoc.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClickDoc.Utils
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<ActAcceptanceTransferWindow>();
            window.Show();
        }

        private void CloseCurrentWindow()
        {
            Application.Current.Windows.OfType<Window>()
                .SingleOrDefault(w => w.IsActive)?
                .Close();
        }
    }
}
