using BargainBot.Model;
using BargainBot.Repositories;

namespace BargainBot
{
    public class TaskHelper
    {
        private IDealRepository _dealRepository;
        private static AmazonClient _amazonClient;

        public TaskHelper(AmazonClient amazonClient, IDealRepository dealRepository)
        {
            _dealRepository = dealRepository;
            _amazonClient = amazonClient;
        }

        public Deal AddOrUpdateDeal(Deal deal)
        {
            var updatedDeal = (Deal)deal.Clone();
            updatedDeal.Price = _amazonClient.GetPriceByAsin(deal.Code);

            var databaseDeal = _dealRepository.GetByCode(deal.Code);
            if (databaseDeal == null)
            {
                // Create deal
                //TODO: Why use the clone and not the real?
                _dealRepository.Create(updatedDeal);

                // Add a task
                //TODO: Create quartz task
            }
            else
            {
                // Check to see if cheaper
                if (databaseDeal.Price < updatedDeal.Price)
                {
                    //TODO: Send message 
                }

            }
            return updatedDeal;
        }

    }
}