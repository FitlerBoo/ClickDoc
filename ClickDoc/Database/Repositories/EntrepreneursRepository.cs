using ClickDoc.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClickDoc.Database.Repositories
{
    public class EntrepreneursRepository : IRepository<EntrepreneurEntity>
    {
        private readonly MyDbContext _context;
        public event Action<EntrepreneurEntity> ItemAdded;
        public event Action<EntrepreneurEntity> ItemRemoved;

        public EntrepreneursRepository(MyDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task Add(EntrepreneurEntity entity)
        {
            _context.Entrepreneurs.Add(entity);
            await _context.SaveChangesAsync();
            ItemAdded?.Invoke(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _context.Entrepreneurs.FindAsync(id);
            if (entity != null)
            {
                _context.Entrepreneurs.Remove(entity);
                await _context.SaveChangesAsync();
                ItemRemoved?.Invoke(entity);
            }
        }

        public async Task<List<EntrepreneurEntity>> GetAll()
            => [.. _context.Entrepreneurs
                .AsNoTracking()];

        public async Task<EntrepreneurEntity> GetById(long id)
            => await _context.Entrepreneurs
                    .FirstOrDefaultAsync(x => x.Id == id);
    }
}
