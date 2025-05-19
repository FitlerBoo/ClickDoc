using ClickDoc.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using System.DirectoryServices;
using System.Windows.Input;

namespace ClickDoc.ViewModels
{
    public class ActAcceptanceTransferVM : ValidatableObject
    {
        private IServiceProvider _serviceProvider;
        private FormData _formData = new ();
        public ICommand CreateCommand { get; }
        public ActAcceptanceTransferVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _formData = _serviceProvider.GetRequiredService<FormData>();
            CreateCommand = new RelayCommand(CreateDocument);
        }

        public decimal UnitCost
        {
            get => _formData.UnitCost;
            set
            {
                _formData.UnitCost = value;
                OnPropertyChanged(nameof(UnitCost));
            }
        }

        public decimal UnitCount
        {
            get => _formData.UnitCount;
            set
            {
                _formData.UnitCount = value;
                OnPropertyChanged(nameof(UnitCount));
            }
        }


        public string ActNumber
        {
            get => _formData.ActNumber;
            set 
            { 
                _formData.ActNumber = value; 
                OnPropertyChanged(nameof(ActNumber)); 
            }
        }

        public string PerformerFullName
        {
            get => _formData.PerformerFullName;
            set 
            { 
                _formData.PerformerFullName = value; 
                OnPropertyChanged(nameof(PerformerFullName)); 
            }
        }

        public string PerformerINN
        {
            get => _formData.PerformerINN;
            set 
            { 
                _formData.PerformerINN = value;
                OnPropertyChanged(nameof(PerformerINN));
            }
        }

        public string ContractNumber
        {
            get => _formData.ContractNumber;
            set
            { 
                _formData.ContractNumber = value;
                OnPropertyChanged(nameof(ContractNumber));
            }
        }

        public string ServiceTypeDescription
        {
            get => _formData.ServiceTypeDescription;
            set { _formData.ServiceTypeDescription = value;
                OnPropertyChanged(nameof(ServiceTypeDescription)); }
        }

        public string InvoiceNumber
        {
            get => _formData.InvoiceNumber;
            set 
            { 
                _formData.InvoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }


        public DateTime ActDate
        {
            get => _formData.ActDate;
            set 
            { 
                _formData.ActDate = value; 
                OnPropertyChanged(nameof(ActDate)); 
            }
        }

        public DateTime ContractDate
        {
            get => _formData.ContractDate;
            set 
            { 
                _formData.ContractDate = value;
                OnPropertyChanged(nameof(ContractDate));
            }
        }

        public DateTime PeriodStart
        {
            get => _formData.PeriodStart;
            set 
            { 
                _formData.PeriodStart = value; 
                OnPropertyChanged(nameof(PeriodStart)); 
            }
        }

        public DateTime PeriodEnd
        {
            get => _formData.PeriodEnd;
            set 
            { 
                _formData.PeriodEnd = value; 
                OnPropertyChanged(nameof(PeriodEnd)); 
            }
        }

        public DateTime InvoiceDate
        {
            get => _formData.InvoiceDate;
            set 
            { 
                _formData.InvoiceDate = value; 
                OnPropertyChanged(nameof(InvoiceDate)); 
            }
        }

        public DateTime LastDate
        {
            get => _formData.LastDate;
            set 
            { 
                _formData.LastDate = value; 
                OnPropertyChanged(nameof(LastDate)); 
            }
        }

        private void CreateDocument()
        {
            var contractData = new AcceptanceTransferActContractData(_formData);

        }
    }
}
