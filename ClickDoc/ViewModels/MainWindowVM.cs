using ClickDoc.Utils;
using ClickDoc.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ClickDoc.ViewModels
{
    public class MainWindowVM : ObservableObject
    {
        private INavigationService _navigationService;

        public ICommand LoadDocumentCommand { get; }
        public ICommand CreateCommand { get; }

        public MainWindowVM(INavigationService ns)
        {
            _navigationService = ns;
            LoadDocumentCommand = new RelayCommand(LoadDocument);
            CreateCommand = new RelayCommand(CreateActForm);
        }

        private void LoadDocument()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Выберите Word документ"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                AnalyzeDocument(filePath);
            }
        }

        private void AnalyzeDocument(string currentDocumentPath)
        {
            throw new NotImplementedException();
        }

        private void CreateActForm()
        {
            _navigationService.NavigateTo<ActAcceptanceTransferWindow>();
        }
    }
}
