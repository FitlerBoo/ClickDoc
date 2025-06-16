namespace ClickDoc.Generators
{
    public interface IGeneratorFactory
    {
        IDocumentGenerator GetGenerator(DocumentGeneratorType type);
        static Dictionary<DocumentGeneratorType, string> GeneratorTypeDisplayNames;
    }
}
