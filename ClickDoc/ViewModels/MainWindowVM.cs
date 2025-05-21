using ClickDoc.Utils;
using ClickDoc.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Windows.Input;

namespace ClickDoc.ViewModels
{
    public class MainWindowVM : ObservableObject
    {
        private readonly INavigationService _navigationService;

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
