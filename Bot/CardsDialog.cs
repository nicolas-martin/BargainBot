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
        private readonly IRepository<User> _userRepo;

        public CardsDialog(IRepository<User> userRepo)
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

                //var resume = new ResumptionCookie(incMessage);

                //var data = JsonConvert.SerializeObject(resume);

                //_userRepo.Create(new User
                //{
                //    Id = Guid.NewGuid(),
                //    ResumptionCookie = data,
                //    Name = resume.UserName,
                //    Deals = new List<Deal> { deal }
                //});

            }
        }
    }
}