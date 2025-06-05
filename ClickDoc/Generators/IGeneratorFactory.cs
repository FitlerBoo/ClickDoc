using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Generators
{
    public interface IGeneratorFactory
    {
        IDocumentGenerator GetGenerator(DocumentGeneratorType type);
        static Dictionary<DocumentGeneratorType, string> GeneratorTypeDisplayNames;
    }
}
