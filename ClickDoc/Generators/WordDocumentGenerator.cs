using ClickDoc.Models;
using Spire.Doc;

namespace ClickDoc.Generators
{
    public class WordDocumentGenerator : IDocumentGenerator
    {
        public async Task GenerateAsync(IContractData contractData, string templatePath, string outputPath)
        {
            await Task.Run(() =>
            {
                using var doc = new Document();
                doc.LoadFromFile(templatePath);


                foreach (var field in contractData.GetFieldNames())
                {
                    string placeholder = $"[{field}]";
                    string value = contractData.GetFieldValue(field) ?? string.Empty;


                    doc.Replace(placeholder, value, false, true);
                }

                doc.SaveToFile(outputPath, FileFormat.Docx2013);
            });
        }
    }
}
