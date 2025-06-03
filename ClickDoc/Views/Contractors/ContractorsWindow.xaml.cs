using ClickDoc.ViewModels.Contractors;
using System.Windows;

namespace ClickDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для ContractorsWindow.xaml
    /// </summary>
    public partial class ContractorsWindow : Window
    {
        public ContractorsWindow(ContractorsVM dc)
        {
            InitializeComponent();
            DataContext = dc;
        }
    }
}
