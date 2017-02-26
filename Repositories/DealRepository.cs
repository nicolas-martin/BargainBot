using System.Linq;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class DealRepository : RepositoryBase<Deal>, IDealRepository
    {
        public DealRepository() : base()
        {
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