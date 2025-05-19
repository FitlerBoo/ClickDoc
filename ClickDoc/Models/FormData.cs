using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Models
{
    public class FormData
    {
        public string ActNumber { get; set; } = string.Empty; // 1

        public DateTime ActDate { get; set; } = DateTime.Now; // 2

        public string PerformerFullName { get; set; } = string.Empty; // 3
        public string PerformerINN { get; set; } = string.Empty; // 4

        public string ContractNumber { get; set; } = string.Empty; // 5
        public DateTime ContractDate { get; set; } = DateTime.Now; // 6

        public DateTime PeriodStart { get; set; } = DateTime.Now; // 7
        public DateTime PeriodEnd { get; set; } = DateTime.Now; // 8

        public string ServiceTypeDescription { get; set; } = string.Empty; // 9
        public decimal UnitCost { get; set; } // 10 
        public decimal UnitCount { get; set; } // 11

        public string InvoiceNumber { get; set; } = string.Empty; // 12
        public DateTime InvoiceDate { get; set; } = DateTime.Now; // 13

        public DateTime LastDate { get; set; } = DateTime.Now; // 14
    }
}
