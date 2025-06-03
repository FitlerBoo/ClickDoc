using ClickDoc.ViewModels.Entrepreneurs;
using System.Windows;

namespace ClickDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для EntrepreneursWindow.xaml
    /// </summary>
    public partial class EntrepreneursWindow : Window
    {
        public EntrepreneursWindow(EntrepreneursVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
