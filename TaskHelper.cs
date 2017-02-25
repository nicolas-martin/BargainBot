using BargainBot.Jobs;
using BargainBot.Model;
using BargainBot.Repositories;

namespace BargainBot
{
    public class TaskHelper
    {
        private IDealRepository _dealRepository;
        private readonly JobScheduler _scheduler;
        private static AmazonClient _amazonClient;

        public TaskHelper(AmazonClient amazonClient, IDealRepository dealRepository, JobScheduler scheduler)
        {
            _dealRepository = dealRepository;
            _scheduler = scheduler;
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
                //TODO: Create quartz task for each or for everything?

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