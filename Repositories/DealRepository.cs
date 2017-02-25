using System;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class DealRepository : IDealRepository
    {
        private Deal _deal = new Deal
        {
            Id = Guid.NewGuid(),
            Code = "123",
            DateCreated = DateTime.Now,
            Name = "TestItem",
            Price = 13.37,
            Url = new Uri("https://www.amazon.ca/Timex-T49905GP-Expedition-Chronograph-Genuine/dp/B009MMINJ2/ref=lp_15610772011_1_1?s=watch&ie=UTF8&qid=1488000037&sr=1-1")
        };

        public Deal Create(Deal obj)
        {
            return _deal;
        }

        public Deal Retreive(Guid id)
        {
            return _deal;
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
            return _deal;
        }

    }

    public interface IDealRepository : IRepository<Deal>
    {
        Deal GetByCode(string code);
    }
}