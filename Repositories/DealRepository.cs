using System;
using System.Linq;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class DealRepository : RepositoryBase<Deal>, IDealRepository
    {
        public DealRepository()
        {
            List.Add
            (
                new Deal
                {
                    Id = Guid.NewGuid(),
                    Code = "123",
                    DateCreated = DateTime.Now,
                    Name = "TestItem",
                    Price = 13.37,
                    Url = new Uri("https://www.amazon.ca/Timex-T49905GP-Expedition-Chronograph-Genuine/dp/B009MMINJ2/ref=lp_15610772011_1_1?s=watch&ie=UTF8&qid=1488000037&sr=1-1")
                }
            );
        }

        //Don't really have to do this but it's nice
        public Deal GetByCode(string code)
        {
            return Find(x => x.Code == code).FirstOrDefault();
        }
    }

    internal interface IDealRepository
    {
        Deal GetByCode(string code);
    }
}