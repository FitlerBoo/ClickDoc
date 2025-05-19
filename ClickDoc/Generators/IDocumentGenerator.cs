using ClickDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Generators
{
    interface IDocumentGenerator
    {
        Task GenerateAsync(IContractData contractData, string templatePath, string outputPdfPath);
    }
}
