using ClickDoc.ViewModels.Contractors;
using System.Windows;

namespace ClickDoc.Views.Contractors
{
    /// <summary>
    /// Логика взаимодействия для NewContractorWindow.xaml
    /// </summary>
    public partial class NewContractorWindow : Window
    {
        public NewContractorWindow(NewContractorVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
