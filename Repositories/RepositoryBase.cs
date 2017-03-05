using System;
using System.Collections.Generic;
using System.Linq;
using BargainBot.Model;
using Microsoft.EntityFrameworkCore;

namespace BargainBot.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IIdentifiable
    {
        internal List<T> List;
        private MyContext _context;

        protected RepositoryBase(MyContext context)
        {
            List = new List<T>();
            _context = context;
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

        public void Create(T obj)
        {
            var entry = _context.Entry<T>(obj);
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public void Update(T obj)
        {
            var entry = _context.Entry<T>(obj);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var obj = Get(id);

            var entry = _context.Entry<T>(obj);
            entry.State = EntityState.Deleted;
            _context.SaveChanges();

        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IQueryable<T> FindAsync(Func<T, bool> predicate)
        {
            return _context.Set<T>();
        }
    }
}