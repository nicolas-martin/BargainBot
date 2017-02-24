using System;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class DealRepository : IDealRepository
    {
        public Deal Create(Deal obj)
        {
            throw new NotImplementedException();
        }

        public Deal Retreive(Guid id)
        {
            throw new NotImplementedException();
        }

        public Deal Update(Deal obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Deal GetByCode(string code)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDealRepository : IRepository<Deal>
    {
        Deal GetByCode(string code);
    }
}