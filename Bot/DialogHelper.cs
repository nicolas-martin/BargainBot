using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BargainBot.Helper;
using BargainBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BargainBot.Bot
{
    public static class DialogHelper
    {

        public static async Task CreateDialogFromMessage(string messageText, string incMessageServiceUrl, ChannelAccount botAccount, ChannelAccount userAccount)
        {
            var connector = new ConnectorClient(new Uri(incMessageServiceUrl));
            var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);
            var message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId.Id);
            message.Text = messageText;
            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);

        }

        public static async Task CreateDialogFromCookie(User user, Deal deal, double updatedDeal)
        {
            dynamic resumeData = JsonConvert.DeserializeObject(user.ResumptionCookie);

            string botId = resumeData.address.botId;
            string channelId = resumeData.address.channelId;
            string userId = resumeData.address.userId;
            string conversationId = resumeData.address.conversationId;
            string serviceUrl = resumeData.address.serviceUrl;
            string userName = resumeData.userName;
            bool isGroup = resumeData.isGroup;

            var resume = new ResumptionCookie(new Address(botId, channelId, userId, conversationId, serviceUrl), userName, isGroup, "en-US");

            var messageactivity = (Activity)resume.GetMessage();
            var client = new ConnectorClient(new Uri(messageactivity.ServiceUrl));

            var reply = messageactivity.CreateReply();
            //reply.Text = $"Hey {userName} ({userId})! I have more *exciting news*! Come back!";

            var pctDiscount = (1 - (updatedDeal / deal.Price)).ToString("0.0%");

            reply.Attachments.Add(GetHeroCard(
                deal.Name,
                $"Discount found!",
                $"You save {pctDiscount} by buying now",
                new CardImage(url: string.Format(Constants.Amazon.FlakyImageUrlPattern, deal.Code)),
                new CardAction(ActionTypes.OpenUrl, "Buy now", value: deal.ShortenUrl)));

            //TODO: Is it possible to use a dialog here?
            //await client.Conversations.ReplyToActivityAsync(reply);
            var newConvo = await client.Conversations.CreateDirectConversationAsync(messageactivity.Recipient, messageactivity.From, reply);
            messageactivity.Conversation.Id = newConvo.Id;

        }

        public static Attachment CreateDealCard(Deal deal)
        {
            return GetHeroCard(
                deal.Name,
                $"Currently sells for {deal.Price}$",
                "We will monitor your item and notify you when it becomes discounted",
                new CardImage(url: deal.ImageUrl),
                new CardAction(ActionTypes.OpenUrl, "View item", value: deal.ShortenUrl));
        }

        public static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
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

        public static IList<Attachment> GetCardsAttachments(List<Deal> userDeals)
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