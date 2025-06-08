using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClickDoc.Database.Repositories
{
    public class ContractsRepository : IRepository<ContractEntity>
    {
        private readonly MyDbContext _context;
        public event Action<ContractEntity> ItemAdded;
        public event Action<ContractEntity> ItemRemoved;

        public ContractsRepository(MyDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task Add(ContractEntity entity)
        {
            _context.Contracts.Add(entity);
            await _context.SaveChangesAsync();
            ItemAdded?.Invoke(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _context.Contracts.FindAsync(id);
            if (entity != null)
            {
                _context.Contracts.Remove(entity);
                await _context.SaveChangesAsync();
                ItemRemoved?.Invoke(entity);
            }
        }

        public async Task<ContractEntity> GetById(long id)
            => await _context.Contracts
                    .Include(x => x.Contractor)
                    .Include(x => x.Entrepreneur)
                    .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ContractEntity>> GetAll()
            => [.. _context.Contracts
                .Include(c => c.Contractor)
                .Include(c => c.Entrepreneur)
                .AsNoTracking()];
    }
}
