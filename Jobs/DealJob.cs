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
            var deals = await _dealRepo.FindAsync().ToListAsync();

            var liveDeals = deals.Where(x => x.IsActive);

            foreach (var liveDeal in liveDeals)
            {
                //var updatedDeal = (Deal)liveDeal.Clone();
                var updatedDeal = _amazonClient.GetPriceByAsin(liveDeal.Code);
                //var updatedDeal = (liveDeal.Price - 1);

                // Check to see if cheaper
                if (updatedDeal < liveDeal.Price)
                {
                    var users = _userRepo.FindAsync();
                    var filteredUsers = users.Where(x => x.Deals.Any(y => y.Code == liveDeal.Code));

                    foreach (var user in filteredUsers)
                    {
                        try
                        {
                            await DialogHelper.CreateDialogFromCookie(user, liveDeal, updatedDeal);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"==Error while creating dialog from cookie\r\n {e}");
                        }
                    }

                }
            }

        }
    }
}