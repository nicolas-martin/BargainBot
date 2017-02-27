using System;
using System.Collections.Generic;
using System.Linq;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : IIdentifiable
    {
        internal List<T> List;

        protected RepositoryBase()
        {
            List = new List<T>();
        }

        public PagedResult<T> RetrievePage(int pageNumber, int pageSize, Func<T, bool> predicate = default(Func<T, bool>))
        {
            var items = this.Find(predicate);

            return new PagedResult<T>
            {
                Items = items.Skip(pageSize * (pageNumber - 1)).Take(pageSize),
                TotalCount = items.Count()
            };
        }

        public T Create(T obj)
        {
            List.Add(obj);
            return obj;
        }

        public T Get(Guid id)
        {
            return List.FirstOrDefault(x => x.Id == id);
        }

        public List<T> Get()
        {
            return List;
        }

        public T Update(T obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            var obj = Get(id);

            List?.Remove(obj);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return predicate != default(Func<T, bool>) ? this.List.Where(predicate) : this.List;
        }

    }
}