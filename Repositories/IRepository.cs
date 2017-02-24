using System;

namespace BargainBot.Repositories
{
    public interface IRepository<T>
    {
        T Create(T obj);
        T Retreive(Guid id);
        T Update(T obj);
        void Delete(Guid id);
    }
}
