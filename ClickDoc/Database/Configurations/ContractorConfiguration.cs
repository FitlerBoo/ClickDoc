using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickDoc.Database.Configurations
{
    public class ContractorConfiguration : IEntityTypeConfiguration<ContractorEntity>
    {
        public void Configure(EntityTypeBuilder<ContractorEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Surname).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Patronymic).HasMaxLength(100);
            builder.Property(x => x.Inn).HasMaxLength(20).IsRequired();

            builder.HasIndex(x => x.Inn).IsUnique();
        }
    }
}
