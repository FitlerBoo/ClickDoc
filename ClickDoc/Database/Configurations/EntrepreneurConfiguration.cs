using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickDoc.Database.Configurations
{
    public class EntrepreneurConfiguration : IEntityTypeConfiguration<EntrepreneurEntity>
    {
        public void Configure(EntityTypeBuilder<EntrepreneurEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Surname).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Patronymic).HasMaxLength(100);
            builder.Property(x => x.OGRNIP).HasMaxLength(15).IsRequired();

            builder.HasIndex(x => x.OGRNIP).IsUnique();
        }
    }
}
