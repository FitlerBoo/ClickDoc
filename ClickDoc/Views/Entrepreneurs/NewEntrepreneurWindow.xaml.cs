using ClickDoc.ViewModels.Entrepreneurs;
using System.Windows;

namespace ClickDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для NewEntrepreneurWindow.xaml
    /// </summary>
    public partial class NewEntrepreneurWindow : Window
    {
        public NewEntrepreneurWindow(NewEntrepreneurVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
