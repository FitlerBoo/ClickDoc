using ClickDoc.Models;

namespace ClickDoc.Generators
{
    interface IDocumentGenerator
    {
        Task GenerateAsync(IContractData contractData, string templatePath, string outputPdfPath);
    }
}
