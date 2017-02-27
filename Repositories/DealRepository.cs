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
                    Url = "https://www.amazon.ca/Timex-T49905GP-Expedition-Chronograph-Genuine/dp/B009MMINJ2/ref=lp_15610772011_1_1?s=watch&ie=UTF8&qid=1488000037&sr=1-1",
                    ShortenUrl = "www.shortUrl.com",
                    ImageUrl = "http://cbsnews1.cbsistatic.com/hub/i/2015/07/11/cd46ace7-1afb-41b2-832b-5b039cc6d9a0/3c77a7f430ce7b492959f0225f6b0afe/forrest-fenn-treasure-chest-620.jpg"
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