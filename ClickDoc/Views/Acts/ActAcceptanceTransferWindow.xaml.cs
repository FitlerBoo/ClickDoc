using ClickDoc.ViewModels;
using System.Windows;

namespace ClickDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для ActAcceptanceTransferWindow.xaml
    /// </summary>
    public partial class ActAcceptanceTransferWindow : Window
    {
        public ActAcceptanceTransferWindow(AcceptanceTransferActVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
