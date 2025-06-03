namespace ClickDoc.Models
{
    public class FormDataTemplate
    {
        private LinkedList<FormField> _fields;

        public void Add(FormField field)
        {
            _fields.AddLast(field);
        }
    }

    public class FormField
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldTag { get; set; } = string.Empty;


    }

    enum FieldType
    {
        Text,
        Number,
        Date
    }
}
