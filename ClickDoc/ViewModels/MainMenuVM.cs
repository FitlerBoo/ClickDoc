using ClickDoc.Utils;
using ClickDoc.Views;
using ClickDoc.Views.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels
{
    public class MainMenuVM : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public ICommand LoadDocumentCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand ManageEntrepreneursCommand { get; }
        public ICommand ManageContractorsCommand { get; }
        public ICommand ManageContractsCommand { get; }

        public MainMenuVM(INavigationService ns)
        {
            _navigationService = ns;
            CreateCommand = new RelayCommand(CreateActForm);
            ManageEntrepreneursCommand = new RelayCommand(ManageEntrepreneurs);
            ManageContractorsCommand = new RelayCommand(ManageContractors);
            ManageContractsCommand = new RelayCommand(ManageContracts);

            LoadDocumentCommand = new RelayCommand(LoadDocument);
        }

        private void ManageContracts()
        {
            _navigationService.NavigateTo<ContractsWindow>();
        }

        private void CreateActForm()
        {
            _navigationService.NavigateTo<ActAcceptanceTransferWindow>();
        }

        private void ManageEntrepreneurs()
        {
            _navigationService.NavigateTo<EntrepreneursWindow>();
        }

        private void ManageContractors()
        {
            _navigationService.NavigateTo<ContractorsWindow>();
        }

        private void LoadDocument()
        {
            MessageBox.Show("Функциональность пока не доступна");
            //var openFileDialog = new OpenFileDialog
            //{
            //    Filter = "Word Documents|*.docx",
            //    Title = "Выберите Word документ"
            //};

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string filePath = openFileDialog.FileName;
            //    AnalyzeDocument(filePath);
            //}
        }

        //private void AnalyzeDocument(string currentDocumentPath)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
