using ClickDoc.Utils;
using Spire.Doc;

namespace ClickDoc.Generators
{
    public class PdfDocumentGenerator(INotificationService notificationService)
        : DocumentGeneratorBase(notificationService)
    {
        protected override FileFormat TargetFormat => FileFormat.PDF;
        protected override string FileExtension => "pdf";
    }
}
