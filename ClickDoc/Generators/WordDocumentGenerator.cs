using ClickDoc.Utils;
using Spire.Doc;

namespace ClickDoc.Generators
{
    public class WordDocumentGenerator(INotificationService notificationService)
        : DocumentGeneratorBase(notificationService)
    {
        protected override FileFormat TargetFormat => FileFormat.Docx2013;
        protected override string FileExtension => "docx";
    }
}
