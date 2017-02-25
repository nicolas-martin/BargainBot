using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BargainBot.Model;
using BargainBot.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Quartz.Util;

namespace BargainBot.Bot
{
    [Serializable]
    public class CardsDialog : IDialog<object>
    {
        private readonly IUserRepository _userRepo;

        public CardsDialog(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var incMessage = await result;

            if (!incMessage.Text.IsNullOrWhiteSpace())
            {

                var resume = new ResumptionCookie(incMessage);

                var data = JsonConvert.SerializeObject(resume);


                var deal = new Deal
                {
                    Id = Guid.NewGuid(),
                    Code = "123",
                    DateCreated = DateTime.Now,
                    Name = "TestItem",
                    Price = 13.37,
                    Url = new Uri("https://www.amazon.ca/Timex-T49905GP-Expedition-Chronograph-Genuine/dp/B009MMINJ2/ref=lp_15610772011_1_1?s=watch&ie=UTF8&qid=1488000037&sr=1-1")
                };

                _userRepo.Create(new User
                {
                    Id = Guid.NewGuid(),
                    ResumptionCookie = data,
                    Name = resume.UserName,
                    Deals = new List<Deal> { deal }
                });

                //TODO: Faking job execution

            }
        }
    }
}