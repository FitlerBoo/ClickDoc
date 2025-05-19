using ClickDoc.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Models
{
    class AcceptanceTransferActContractData : IContractData
    {
        private readonly Dictionary<string, string> _fields;
        [Required] public string? ActNumber { get; set; } // 1

        [Required] public string? ActDate { get; set; } // 2

        [Required] public string? PerformerFullName {  get; set; } // 3
        [Required] public string? PerformerINN { get; set; }

        [Required] public string? ContractNumber { get; set; } // 4
        [Required] public string? ContractDate { get; set; }


        [Required] public string? PeriodStart { get; set; } // 5
        [Required] public string? PeriodEnd { get; set; }

        [Required] public string? ServiceType { get; set; } // 6
        [Required] public string? UnitCost { get; set; }
        [Required] public string? UnitCount { get; set; }
        [Required] public string? TotalCost { get; set; }

        [Required] public string? InvoiceNumber { get; set; } // 7
        [Required] public string? InvoiceDate { get; set; }

        [Required] public string? PriceInText { get; set; } // 8

        [Required] public string? LastDate { get; set; } // 9
        [Required] public string? PerformerSurnameInitials { get; set; }



        public AcceptanceTransferActContractData(FormData formData)
            => _fields = ConvertFormData(formData);

        public string GetFieldValue(string fieldName)
            => _fields.TryGetValue(fieldName, out var value) ? value : null;

        public IEnumerable<string> GetFieldNames() => _fields.Keys;

        public bool Validate(out IEnumerable<string> errors)
        {
            var errs = new List<string>();

            if (string.IsNullOrEmpty(_fields["ActNumber"]))
                errs.Add("Номер акта обязателен");

            // Другие проверки...

            errors = errs;
            return !errs.Any();
        }

        private static Dictionary<string, string> ConvertFormData(FormData data)
        {
            return new()
            {
                ["ActNumber"] = data.ActNumber,

                ["ActDate"] = DateFormatter.FormatDateWithQuotedDay(data.ActDate),

                ["PerformerFullName"] = data.PerformerFullName,
                ["PerformerINN"] = data.PerformerINN,

                ["ContractNumber"] = data.ContractNumber,
                ["ContractDate"] = DateFormatter.FormatShort(data.ContractDate),

                ["PeriodStart"] = DateFormatter.FormatLong(data.PeriodStart),
                ["PeriodEnd"] = DateFormatter.FormatLong(data.PeriodEnd),

                ["ServiceType"] = data.ServiceTypeDescription,
                ["UnitCost"] = data.UnitCost.ToString("N2", CultureInfo.CurrentCulture),
                ["UnitCount"] = data.UnitCount.ToString(),
                ["TotalCost"] = (data.UnitCount * data.UnitCost).ToString("N2", CultureInfo.CurrentCulture),

                ["InvoiceNumber"] = data.InvoiceNumber,
                ["InvoiceDate"] = DateFormatter.FormatLong(data.InvoiceDate),

                ["PriceInText"] = CostFormatter.CostToWordsFormat(data.UnitCount * data.UnitCost),

                ["LastDate"] = DateFormatter.FormatShort(data.LastDate),
                ["PerformerSurnameInitials"] = FullnameFormatter.GetInitials(data.PerformerFullName)
            };
        }
    }
}
