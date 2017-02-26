using System.Collections.Generic;
using System.Linq;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class DealRepository : RepositoryBase<Deal>, IDealRepository
    {
        private List<Deal> _deals;

        public Deal GetByCode(string code)
        {
            return _deals.FirstOrDefault(x => x.Code == code);
        }
    }

    internal interface IDealRepository
    {
        Deal GetByCode(string code);
    }
}