using System.ComponentModel.DataAnnotations;

namespace ClickDoc.Database.Entities
{
    public class EntrepreneurEntity : Entity
    {
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string OGRNIP { get; set; } = string.Empty;
        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();
    }
}
