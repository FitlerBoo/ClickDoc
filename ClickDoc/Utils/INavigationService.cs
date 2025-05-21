using System.Windows;

namespace ClickDoc.Utils
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Window;
        void CloseCurrentWindow();
    }
}
