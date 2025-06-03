using ClickDoc.ViewModels.Contracts;
using System.Windows;

namespace ClickDoc.Views.Contracts
{
    /// <summary>
    /// Логика взаимодействия для NewContractWindow.xaml
    /// </summary>
    public partial class NewContractWindow : Window
    {
        public NewContractWindow(NewContractVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
