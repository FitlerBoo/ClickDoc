using ClickDoc.Utils;
using System.Globalization;

namespace ClickDoc.Models
{
    class AcceptanceTransferActContractData(FormData formData) : IContractData
    {
        private readonly Dictionary<string, string> _fields = ConvertFormData(formData);

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
