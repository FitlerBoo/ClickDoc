using ClickDoc.ViewModels;
using System.Windows;

namespace ClickDoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}