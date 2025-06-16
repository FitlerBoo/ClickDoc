using ClickDoc.Utils;

namespace ClickDoc.Generators
{
    public class GeneratorFactory(INotificationService notificationService) : IGeneratorFactory
    {
        private readonly INotificationService _notificationService = notificationService;

        public IDocumentGenerator GetGenerator(DocumentGeneratorType type)
            => type switch
            {
                DocumentGeneratorType.Pdf => new PdfDocumentGenerator(_notificationService),
                DocumentGeneratorType.Word => new WordDocumentGenerator(_notificationService),
                _ => new WordDocumentGenerator(_notificationService)
            };

        public static Dictionary<DocumentGeneratorType, string> GeneratorTypeDisplayNames => new()
        {
            { DocumentGeneratorType.Pdf, "PDF документ" },
            { DocumentGeneratorType.Word, "Word документ" }
        };
    }

    public enum DocumentGeneratorType
    {
        Pdf,
        Word
    }
}
