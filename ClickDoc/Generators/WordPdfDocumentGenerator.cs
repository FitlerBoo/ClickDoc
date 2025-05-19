using ClickDoc.Models;
using iTextSharp.text.log;
using System.IO;
using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace ClickDoc.Generators
{
    class WordPdfDocumentGenerator : IDocumentGenerator, IDisposable
    {
        private readonly Word.Application _wordApp;
        private bool _disposed;
        public WordPdfDocumentGenerator()
        {
            _wordApp = new Word.Application { Visible = false };
        }

        public async Task GenerateAsync(IContractData contractData, string templatePath, string outputPath)
        {

            await Task.Run(() =>
            {
                if (contractData == null) throw
                    new ArgumentNullException(nameof(contractData));
                if (!File.Exists(templatePath))
                    throw new FileNotFoundException("Template not found", templatePath);

                Word.Document doc = null;

                try
                {
                    doc = _wordApp.Documents.Open(templatePath);
                    var fields = contractData.GetFieldNames();

                    foreach (var fieldName in contractData.GetFieldNames())
                    {
                            if (doc.Bookmarks.Exists(fieldName))
                            {
                                var value = contractData.GetFieldValue(fieldName) ?? string.Empty;
                                SetBookmarkValue(doc, fieldName, value);
                            }
                    }

                    doc.ExportAsFixedFormat(outputPath, Word.WdExportFormat.wdExportFormatPDF);
                }
                finally
                {
                    doc?.Close(false);
                    ReleaseComObject(doc);
                }
            });
        }

        private void SetBookmarkValue(Word.Document doc, string bookmarkName, string value)
        {
            object name = bookmarkName;
            var range = doc.Bookmarks.get_Item(ref name).Range;
            range.Text = value;
            doc.Bookmarks.Add(bookmarkName, range);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing && _wordApp != null)
            {
                _wordApp.Quit();
                ReleaseComObject(_wordApp);
            }
            _disposed = true;
        }

        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    Marshal.FinalReleaseComObject(obj);
                }
            }
            catch { /* Логирование */ }
        }
    }
}
