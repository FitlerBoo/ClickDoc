namespace ClickDoc.Models
{
    public interface IContractData
    {
        string GetFieldValue(string fieldName);
        IEnumerable<string> GetFieldNames();
    }
}
