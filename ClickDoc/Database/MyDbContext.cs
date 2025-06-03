using ClickDoc.Database.Configurations;
using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClickDoc.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<EntrepreneurEntity> Entrepreneurs { get; set; }
        public DbSet<ContractorEntity> Contractors { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContractorConfiguration());
            modelBuilder.ApplyConfiguration(new EntrepreneurConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ClickDoc.db");
        }
    }
}
