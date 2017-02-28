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
                    Code = "B00U5UE146",
                    DateCreated = DateTime.Now,
                    Name = "TestItem",
                    Price = 13.37,
                    Url = "https://www.amazon.com/Nerf-N-Strike-Modulus-ECS-10-Blaster/dp/B00U5UE146/ref=s9u_cartx_gw_i2?_encoding=UTF8&fpl=fresh&pf_rd_m=ATVPDKIKX0DER&pf_rd_s=&pf_rd_r=BSMJRD0RA46EC8D2XXPD&pf_rd_t=36701&pf_rd_p=1cded295-23b4-40b1-8da6-7c1c9eb81d33&pf_rd_i=desktop",
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