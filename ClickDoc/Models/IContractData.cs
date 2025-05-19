using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Models
{
    public interface IContractData
    {
        string GetFieldValue(string fieldName);

        IEnumerable<string> GetFieldNames();

        bool Validate(out IEnumerable<string> errors);
    }
}
