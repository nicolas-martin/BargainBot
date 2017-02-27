using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BargainBot.Client;
using BargainBot.Helper;
using BargainBot.Model;
using BargainBot.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Quartz.Util;

namespace BargainBot.Bot
{
    [Serializable]
    public class CardsDialog : IDialog<object>
    {
        private readonly IRepository<User> _userRepo;
        private readonly AmazonClient _amazonClient;

        public CardsDialog(IRepository<User> userRepo, AmazonClient amazonClient)
        {
            _userRepo = userRepo;
            _amazonClient = amazonClient;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var incMessage = await result;

            PromptDialog.Text(
                context,
                DisplayConfirmDeal,
                "Please paste the url of the item you would like to monitor",
                "Could not validate amazon URL");

        }

        public async Task DisplayConfirmDeal(IDialogContext context, IAwaitable<string> result)
        {
            var amazonURL = await result;

            var asin = UrlValidator.GetAsin(amazonURL);

            if (asin.IsNullOrWhiteSpace())
            {
                context.Fail(new Exception("Could not validate the amazon url"));
            }

            var message = context.MakeMessage();
            var item = _amazonClient

            message.Attachments.Add(CreateDealCard(asin));

            await context.PostAsync(message);

            //TODO: Create deal and user in repo 
            //var resume = new ResumptionCookie(incMessage);

            //var data = JsonConvert.SerializeObject(resume);

            //_userRepo.Create(new User
            //{
            //    Id = Guid.NewGuid(),
            //    ResumptionCookie = data,
            //    Name = resume.UserName,
            //    Deals = new List<Deal> { deal }
            //});


            //TODO: Where should the dialog go from here?
            context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment CreateDealCard(Deal deal)
        {

            //TODO: Fetch deal data from amazon.

            var heroCard = new HeroCard
            {
                Title = "Nice stuff",
                Subtitle = $"The asin is {deal.Code}",
                Text = "We will monitor your item and notify you when it becomes discounted",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "View item", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }

    }
}