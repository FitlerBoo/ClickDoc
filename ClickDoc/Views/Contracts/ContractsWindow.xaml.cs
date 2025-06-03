using ClickDoc.ViewModels.Contracts;
using System.Windows;

namespace ClickDoc.Views.Contracts
{
    public partial class ContractsWindow : Window
    {
        public ContractsWindow(ContractsVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
