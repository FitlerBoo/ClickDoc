using ClickDoc.Models;

namespace ClickDoc.Generators
{
    public interface IDocumentGenerator
    {
        Task GenerateAsync(IContractData contractData, string templatePath, string outputPath);
    }
}
