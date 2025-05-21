namespace ClickDoc.Models
{
    public interface IContractData
    {
        string GetFieldValue(string fieldName);

        IEnumerable<string> GetFieldNames();

        bool Validate(out IEnumerable<string> errors);
    }
}
