namespace ClickDoc.Database.Entities
{
    public class ContractEntity : Entity
    {
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public long ContractorId { get; set; }
        public ContractorEntity Contractor { get; set; }

        public long EntrepreneurId { get; set; }
        public EntrepreneurEntity Entrepreneur { get; set; }

        public string GetInfo
            => $"{ContractNumber} между {Entrepreneur.ShortName} и {Contractor.ShortName}";
    }
}
