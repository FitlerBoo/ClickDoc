using ClickDoc.ViewModels;
using System.Windows;

namespace ClickDoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow(MainMenuVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}