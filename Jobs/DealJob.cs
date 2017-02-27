using System;
using System.Diagnostics;
using BargainBot.Bot;
using BargainBot.Client;
using BargainBot.Model;
using BargainBot.Repositories;
using Quartz;

namespace BargainBot.Jobs
{
    public class DealJob : IJob
    {
        private IRepository<Deal> _dealRepo;
        private IRepository<User> _userRepo;
        private AmazonClient _amazonClient;

        public DealJob(AmazonClient amazonClient, IRepository<Deal> dealRepo, IRepository<User> userRepo)
        {
            _dealRepo = dealRepo;
            _userRepo = userRepo;
            _amazonClient = amazonClient;
        }

        void IJob.Execute(IJobExecutionContext context)
        {
            var liveDeals = _dealRepo.Get();

            //var obj = context.JobDetail.JobDataMap["k"];
            Debug.WriteLine("Parsing deals.");

            foreach (var liveDeal in liveDeals)
            {
                var updatedDeal = (Deal)liveDeal.Clone();
                updatedDeal.Price = _amazonClient.GetPriceByAsin(liveDeal.Code);

                // Check to see if cheaper
                if (liveDeal.Price < updatedDeal.Price)
                {
                    var users = _userRepo.Get();

                    foreach (var user in users)
                    {
                        //uhhh...
                        var t = CreateDialogHelper.CreateDialogFromCookie(user.ResumptionCookie);
                    }

                }

            }
        }
    }
}