using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BargainBot.Dialog
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IBargainBotDialogFactory _dialogFactory;
        private readonly IRepository<User> _userRepo;
        private readonly AmazonClient _amazonClient;
        private ResumptionCookie _cookie;

        public RootDialog(IBargainBotDialogFactory dialogFactory,IRepository<User> userRepo, AmazonClient amazonClient)
        {
            _dialogFactory = dialogFactory;
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
                    reply.Attachments = DialogHelper.GetCardsAttachments(liveUserDeals.Deals);

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

                    message.Attachments.Add(DialogHelper.CreateDealCard(item));

                    var data = JsonConvert.SerializeObject(_cookie);

                    //TODO: Using name is probably not very safe.. 
                    //Use internalId instead?
                    var user = _userRepo.Find(x => x.Name == _cookie.UserName).FirstOrDefault();
                    if (user == null)
                    {
                        Debug.WriteLine($"New user. Creating {_cookie.UserName} with {item.Name}");
                        _userRepo.Create(new User
                        {
                            ResumptionCookie = data,
                            Name = _cookie.UserName,
                            InternalId = context.Activity.From.Id,
                            Deals = new List<Deal> {item}
                        });
                    }
                    else
                    {
                        Debug.WriteLine($"Existing user. Updating {user.Name} with {item.Name}");
                        if (user.Deals.All(x => x.Code != item.Code))
                        {
                            user.Deals.Add(item);
                            user.ResumptionCookie = data;
                            _userRepo.Update(user);
                        }
                        else
                        {
                            await context.PostAsync($"Already tracking {item.Name}");
                            context.Wait(this.MessageReceivedAsync);
                        }

                    }

                    await context.PostAsync(message);
                }

            }
        }

    }
}