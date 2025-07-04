﻿using ClickDoc.Utils;
using System.Globalization;

namespace ClickDoc.Models
{
    class AcceptanceTransferActContractData(FormData formData) : IContractData
    {
        private readonly Dictionary<string, string> _fields = ConvertFormData(formData);

        public string GetFieldValue(string fieldName)
            => _fields.TryGetValue(fieldName, out var value) ? value : null;

        public IEnumerable<string> GetFieldNames() => _fields.Keys;

        private static Dictionary<string, string> ConvertFormData(FormData data)
        {
            return new()
            {
                ["ActNumber"] = data.ActNumber,

                ["ActDate"] = DateFormatter.FormatDateWithQuotedDay(data.ActDate),

                ["EntrepreneurFullName"] = data.EntrepreneurFullName,
                ["EntrepreneurOGRNIP"] = data.OGRNIP,

                ["PerformerFullName"] = data.ContractorFullName,
                ["PerformerINN"] = data.ContractorINN,

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

                ["LastDate"] = DateFormatter.FormatShort(data.SingingDate),
                ["EntrepreneurSurnameInitials"] = FullnameFormatter.GetInitials(data.EntrepreneurFullName),
                ["ContractorSurnameInitials"] = FullnameFormatter.GetInitials(data.ContractorFullName)
            };
        }
    }
}
