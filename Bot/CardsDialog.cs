using System;
using System.Collections.Generic;
using System.Linq;
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

            if (string.Equals(incMessage.Text, "view deals", StringComparison.InvariantCultureIgnoreCase))
            {
                var liveUserDeals = _userRepo.Get().FirstOrDefault(x => x.Name == context.Activity.From.Name);

                if (liveUserDeals == null)
                {
                    var noDealReply = context.MakeMessage();
                    noDealReply.Text = "No deals found";
                    await context.PostAsync(noDealReply);
                }
                else
                {
                    var reply = context.MakeMessage();

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments = GetCardsAttachments(liveUserDeals.Deals);

                    await context.PostAsync(reply);
                }

            }
            else
            {
                PromptDialog.Text(
                    context,
                    DisplayConfirmDeal,
                    "Please paste the url of the item you would like to monitor",
                    "Could not validate amazon URL");
            }

        }

        public async Task DisplayConfirmDeal(IDialogContext context, IAwaitable<string> result)
        {
            var amazonUrl = await result;

            if (!UrlValidator.IsCountryFromUrlAllowed(amazonUrl))
            {
                await context.PostAsync("Sorry, we only accept links from amazon.com");
                context.Wait(this.MessageReceivedAsync);
            }
            //Not the best way to handle exception, but I don't know any other way.
            else
            {
                var asin = UrlValidator.GetAsin(amazonUrl);

                if (asin.IsNullOrWhiteSpace())
                {
                    await context.PostAsync("Could not validate the amazon url");
                    context.Wait(this.MessageReceivedAsync);
                }
                //Not the best way to handle exception, but I don't know any other way.
                else
                {
                    var message = context.MakeMessage();
                    var item = _amazonClient.GetDeal(asin);

                    message.Attachments.Add(CreateDealCard(item));


                    var data = JsonConvert.SerializeObject(_cookie);

                    //TODO: Using name is probably not very safe..
                    var user = _userRepo.Find(x => x.Name == _cookie.UserName).FirstOrDefault();
                    if (user == null)
                    {

                        _userRepo.Create(new User
                        {
                            ResumptionCookie = data,
                            Name = _cookie.UserName,
                            Deals = new List<Deal> {item}
                        });
                    }
                    else
                    {
                        user.Deals.Add(item);
                        user.ResumptionCookie = data;
                        _userRepo.Update(user);
                    }


                    await context.PostAsync(message);
                }

            }
            //context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment CreateDealCard(Deal deal)
        {
            var imgUrl = string.Format(Constants.Amazon.FlakyImageUrlPattern, deal.Code);
            return GetHeroCard(
                deal.Name,
                $"Currently sells for {deal.Price}$",
                "We will monitor your item and notify you when it becomes discounted",
                new CardImage(url: imgUrl),
                new CardAction(ActionTypes.OpenUrl, "View item", value: deal.ShortenUrl));
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

        public IList<Attachment> GetCardsAttachments(List<Deal> userDeals)
        {
            var attachements = new List<Attachment>();
            foreach (var deal in userDeals.Where(x => x.IsActive))
            {
                attachements.Add(CreateDealCard(deal));
            }

            return attachements;
        }


    }
}