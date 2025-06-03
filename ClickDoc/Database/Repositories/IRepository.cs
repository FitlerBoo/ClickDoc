using ClickDoc.Database.Entities;

namespace ClickDoc.Database.Repositories
{
    interface IRepository<T> where T : class
    {
        event Action<T> ItemAdded;
        event Action<T> ItemRemoved;
        Task<List<T>> GetAll();
        Task Add(T entity);
        Task Delete(long id);
        Task<T> GetById(long id);
    }
}
