using ClickDoc.Models;
using ClickDoc.Utils;

namespace ClickDoc.Generators
{
    public interface IDocumentGenerator
    {
        Task GenerateAsync(IContractData contractData, string templatePath, string filename);
    }
}
