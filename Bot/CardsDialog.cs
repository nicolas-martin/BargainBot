using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BargainBot.Client;
using BargainBot.Helper;
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
        private readonly AmazonClient _amazonClient;
        private ResumptionCookie _cookie;

        public CardsDialog(IRepository<User> userRepo, AmazonClient amazonClient)
        {
            _userRepo = userRepo;
            _amazonClient = amazonClient;
            _cookie = null;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var incMessage = await result;

            if (_cookie == null)
            {
                _cookie = new ResumptionCookie(incMessage);
            }

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
            var item = _amazonClient.GetDeal(asin);

            message.Attachments.Add(CreateDealCard(item));

            await context.PostAsync(message);

            var data = JsonConvert.SerializeObject(_cookie);

            _userRepo.Create(new User
            {
                ResumptionCookie = data,
                Name = _cookie.UserName,
                Deals = new List<Deal> { item }
            });

            //TODO: Where should the dialog go from here?
            context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment CreateDealCard(Deal deal)
        {
            var imgUrl = string.Format(Constants.Amazon.FlakyImageUrlPattern, deal.Code);
            var heroCard = new HeroCard
            {
                Title = deal.Name,
                Subtitle = $"Currently sells for {deal.Price}$",
                Text = "We will monitor your item and notify you when it becomes discounted",
                Images = new List<CardImage> { new CardImage(imgUrl) },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "View item", value: deal.ShortenUrl) }
            };

            return heroCard.ToAttachment();
        }

    }
}