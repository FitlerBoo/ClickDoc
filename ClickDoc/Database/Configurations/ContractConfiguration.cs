using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickDoc.Database.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ContractNumber)
                  .HasMaxLength(100)
                  .IsRequired();

            builder.HasOne(x => x.Contractor)
                   .WithMany()
                   .HasForeignKey(x => x.ContractorId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasOne(x => x.Entrepreneur)
                   .WithMany()
                   .HasForeignKey(x => x.EntrepreneurId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasIndex(x => x.ContractNumber).IsUnique();
        }
    }
}
