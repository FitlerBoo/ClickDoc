namespace ClickDoc.Models
{
    public class FormData
    {
        public string ActNumber { get; set; } = string.Empty; // 1

        public DateTime ActDate { get; set; } = DateTime.Now; // 2

        public string EntrepreneurFullName { get; set; } = string.Empty;
        public string OGRNIP { get; set; } = string.Empty;

        public string ContractorFullName { get; set; } = string.Empty; // 3
        public string ContractorINN { get; set; } = string.Empty; // 4

        public string ContractNumber { get; set; } = string.Empty; // 5
        public DateTime ContractDate { get; set; } = DateTime.Now; // 6

        public DateTime PeriodStart { get; set; } = DateTime.Now; // 7
        public DateTime PeriodEnd { get; set; } = DateTime.Now; // 8

        public string ServiceTypeDescription { get; set; } = string.Empty; // 9
        public decimal UnitCost { get; set; } // 10 
        public decimal UnitCount { get; set; } // 11

        public string InvoiceNumber { get; set; } = string.Empty; // 12
        public DateTime InvoiceDate { get; set; } = DateTime.Now; // 13

        public DateTime SingingDate { get; set; } = DateTime.Now; // 14
    }
}
