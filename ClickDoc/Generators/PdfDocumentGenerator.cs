using ClickDoc.Models;
using ClickDoc.Utils;
using Spire.Doc;
using System.DirectoryServices.ActiveDirectory;
using System.IO;

namespace ClickDoc.Generators
{
    public class PdfDocumentGenerator(INotificationService notificationService) : IDocumentGenerator
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
                        $"{filename}.pdf");
                    doc.SaveToFile(outputPath, FileFormat.PDF);
                }
                catch (Exception ex)
                {
                    _notificationService.ShowError($"Ошибка формирования документа:\n{ex.Message}");
                }
            });
        }
    }
}
