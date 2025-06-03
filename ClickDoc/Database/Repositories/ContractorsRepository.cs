using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClickDoc.Database.Repositories
{
    public class ContractorsRepository : IRepository<ContractorEntity>
    {
        private readonly MyDbContext _context;
        public event Action<ContractorEntity> ItemAdded;
        public event Action<ContractorEntity> ItemRemoved;

        public ContractorsRepository(MyDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task Add(ContractorEntity entity)
        {
            _context.Contractors.Add(entity);
            await _context.SaveChangesAsync();
            ItemAdded?.Invoke(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _context.Contractors.FindAsync(id);
            if (entity != null)
            {
                _context.Contractors.Remove(entity);
                await _context.SaveChangesAsync();
                ItemRemoved?.Invoke(entity);
            }
        }

        public async Task<List<ContractorEntity>> GetAll()
            => [.. _context.Contractors
                .AsNoTracking()];

        public async Task<ContractorEntity> GetById(long id)
            => await _context.Contractors
                    .FirstOrDefaultAsync(x => x.Id == id);
    }
}
