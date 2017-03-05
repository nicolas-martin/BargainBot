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
            //TODO: Deal != cancelled
            var liveDeals = await _dealRepo.FindAsync(x => !x.Name.IsNullOrWhiteSpace()).ToListAsync();

            foreach (var liveDeal in liveDeals)
            {
                //var updatedDeal = (Deal)liveDeal.Clone();
                var updatedDeal = _amazonClient.GetDeal(liveDeal.Code);

                // Check to see if cheaper
                if ((liveDeal.Price - 1) < liveDeal.Price)
                {
                    var users = await _userRepo.FindAsync(x =>
                    {
                        Deal first = null;
                        foreach (var d in x.Deals)
                        {
                            if (d.Code == liveDeal.Code)
                            {
                                first = d;
                                break;
                            }
                        }
                        return first;
                    }).ToListAsync();

                    foreach (var user in users)
                    {
                        //uhhh...
                        await CreateDialogHelper.CreateDialogFromCookie(user.ResumptionCookie);
                    }

                }
            }

        }
    }
}