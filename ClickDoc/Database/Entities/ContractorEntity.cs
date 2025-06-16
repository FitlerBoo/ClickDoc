namespace ClickDoc.Database.Entities
{
    public class ContractorEntity : Entity
    {
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Inn { get; set; } = string.Empty;
        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

    }
}
