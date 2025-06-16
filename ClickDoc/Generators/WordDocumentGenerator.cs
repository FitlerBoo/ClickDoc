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

                    var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var baseFileName = Path.Combine(desktopPath, filename);
                    var outputPath = GetUniqueFileName(baseFileName, "docx");
                    doc.SaveToFile(outputPath, FileFormat.Docx2013);
                    _notificationService.ShowSuccess($"Файл {filename} успешно создан на рабочем столе");
                }
                catch (Exception ex)
                {
                    _notificationService.ShowError($"Ошибка формирования документа:\n{ex.Message}");
                }
            });
        }

        private string GetUniqueFileName(string basePath, string extension)
        {
            var counter = 1;
            var newPath = $"{basePath}.{extension}";

            while (File.Exists(newPath))
            {
                newPath = $"{basePath} ({counter}).{extension}";
                counter++;
            }

            return newPath;
        }
    }
}
