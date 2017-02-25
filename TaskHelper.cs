using System;
using BargainBot.Bot;
using BargainBot.Model;
using BargainBot.Repositories;

namespace BargainBot
{
    public class TaskHelper
    {
        private IDealRepository _dealRepo;
        private IUserRepository _userRepo;
        private static AmazonClient _amazonClient;

        public TaskHelper(AmazonClient amazonClient, IDealRepository dealRepo, IUserRepository userRepo)
        {
            _dealRepo = dealRepo;
            _userRepo = userRepo;
            _amazonClient = amazonClient;
        }

        public Deal AddOrUpdateDeal(Deal deal)
        {
            var updatedDeal = (Deal)deal.Clone();
            updatedDeal.Price = _amazonClient.GetPriceByAsin(deal.Code);

            var databaseDeal = _dealRepo.GetByCode(deal.Code);
            if (databaseDeal == null)
            {
                // Create deal
                _dealRepo.Create(updatedDeal);
            }
            else
            {
                // Check to see if cheaper
                if (databaseDeal.Price < updatedDeal.Price)
                {
                    //TODO: How do we get the user id?
                    var user = _userRepo.Retreive(Guid.NewGuid());
                    CreateDialog.CreateDialogFromCookie(user.ResumptionCookie);
                }

            }
            return updatedDeal;
        }

    }
}