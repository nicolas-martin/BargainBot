using System;
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

                // Persist this ResumptionCookie somewhere  

                var data = JsonConvert.SerializeObject(resume);
                _userRepo.Create(new User
                {
                    Id = Guid.NewGuid(),
                    ResumptionCookie = data,
                    Name = resume.UserName
                });

                await CreateDialog.CreateDialogFromCookie(data);

            }
        }
    }
}