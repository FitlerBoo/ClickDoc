using ClickDoc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClickDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для ActAcceptanceTransferWindow.xaml
    /// </summary>
    public partial class ActAcceptanceTransferWindow : Window
    {
        public ActAcceptanceTransferWindow(ActAcceptanceTransferVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
