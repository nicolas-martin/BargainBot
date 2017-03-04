using System;
using System.Collections.Generic;

namespace BargainBot.Repositories
{
    public interface IRepository<T>
    {
        void Create(T obj);
        T Get(Guid id);
        IEnumerable<T> Get();
        void Update(T obj);
        void Delete(Guid id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        PagedResult<T> RetrievePage(int pageNumber, int pageSize, Func<T, bool> predicate = default(Func<T, bool>));
    }
}
