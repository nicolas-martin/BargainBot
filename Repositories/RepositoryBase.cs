using System;
using System.Collections.Generic;
using System.Linq;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : IIdentifiable
    {
        private List<T> _list;

        protected RepositoryBase()
        {
            _list = new List<T>();
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
            _list.Add(obj);
            return obj;
        }

        public T Get(Guid id)
        {
            return _list.FirstOrDefault(x => x.Id == id);
        }

        public List<T> Get()
        {
            return _list;
        }

        public T Update(T obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            var obj = Get(id);

            _list?.Remove(obj);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return predicate != default(Func<T, bool>) ? this._list.Where(predicate) : this._list;
        }

    }
}