using ClickDoc.Generators;

namespace ClickDoc.Utils
{
    public static class GeneratorFactory
    {
        public static IDocumentGenerator GetGenerator(DocumentGeneratorType type)
            => type switch
            {
                DocumentGeneratorType.Pdf => new PdfDocumentGenerator(),
                DocumentGeneratorType.Word => new WordDocumentGenerator(),
                _ => new WordDocumentGenerator()
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
