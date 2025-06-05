using ClickDoc.Models;
using ClickDoc.Utils;
using Spire.Doc;
using System.IO;

namespace ClickDoc.Generators
{
    public class WordDocumentGenerator(INotificationService notificationService) : IDocumentGenerator
    {
        private readonly INotificationService _notificationService = notificationService;

        public async Task GenerateAsync(IContractData contractData, string templatePath, string filename)
        {
            await Task.Run(() =>
            {
                try
                {
                    using var doc = new Document();
                    doc.LoadFromFile(templatePath);
                    foreach (var field in contractData.GetFieldNames())
                    {
                        string placeholder = $"[{field}]";
                        string value = contractData.GetFieldValue(field) ?? string.Empty;
                        doc.Replace(placeholder, value, false, true);
                    }
                    var outputPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        $"{filename}.docx");
                    doc.SaveToFile(outputPath, FileFormat.Docx2013);
                }
                catch (Exception ex)
                {
                    _notificationService.ShowError($"Ошибка формирования документа:\n{ex.Message}");
                }
            });
        }
    }
}
