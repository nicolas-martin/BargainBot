using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BargainBot.Bot;
using BargainBot.Client;
using BargainBot.Model;
using BargainBot.Repositories;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Util;

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

            //var obj = context.JobDetail.JobDataMap["k"];
            Debug.WriteLine("Parsing deals.");
            NewMethod();

        }

        private async void NewMethod()
        {
            var liveDeals = await _dealRepo.FindAsync(x => x.IsActive).ToListAsync();

            foreach (var liveDeal in liveDeals)
            {
                //var updatedDeal = (Deal)liveDeal.Clone();
                //var updatedDeal = _amazonClient.GetDeal(liveDeal.Code);
                var updatedDeal = (liveDeal.Price - 1);

                // Check to see if cheaper
                if (updatedDeal < liveDeal.Price)
                {
                    var users = _userRepo.Get().Where(x => x.Deals.Any(y => y.Code == liveDeal.Code));

                    foreach (var user in users)
                    {
                        try
                        {
                            await DialogHelper.CreateDialogFromCookie(user, liveDeal, updatedDeal);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }

                }
            }

        }
    }
}