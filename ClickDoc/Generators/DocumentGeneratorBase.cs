using ClickDoc.Models;
using ClickDoc.Utils;
using Microsoft.Win32;
using Spire.Doc;
using System.IO;

namespace ClickDoc.Generators
{
    public abstract class DocumentGeneratorBase(INotificationService notificationService) : IDocumentGenerator
    {
        protected readonly INotificationService _notificationService = notificationService;
        protected abstract FileFormat TargetFormat { get; }
        protected abstract string FileExtension { get; }
        public async Task GenerateAsync(IContractData contractData, string templatePath, string filename)
        {
            await Task.Run(() =>
            {
                try
                {
                    using var doc = new Document();
                    doc.LoadFromFile(templatePath);
                    ProcessDocument(contractData, doc);

                    var outputPath = GetOutputPath(filename);
                    doc.SaveToFile(outputPath, TargetFormat);

                    _notificationService.ShowSuccess($"Файл {Path.GetFileName(outputPath)} успешно создан");
                }
                catch (Exception ex)
                {
                    _notificationService.ShowError($"Ошибка формирования документа:\n{ex.Message}");
                }
            });
        }

        private static void ProcessDocument(IContractData contractData, Document doc)
        {
            foreach (var field in contractData.GetFieldNames())
            {
                string placeholder = $"[{field}]";
                string value = contractData.GetFieldValue(field) ?? string.Empty;
                doc.Replace(placeholder, value, false, true);
            }
        }

        protected virtual string GetOutputPath(string filename)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = $"{FileExtension.ToUpper()} Files|*.{FileExtension}",
                DefaultExt = $".{FileExtension}",
                FileName = filename
            };

            if (saveFileDialog.ShowDialog() != true)
                throw new OperationCanceledException("Пользователь отменил сохранение файла");

            return saveFileDialog.FileName;
        }
    }
}
