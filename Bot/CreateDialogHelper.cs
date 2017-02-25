using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BargainBot.Bot
{
    public static class CreateDialogHelper
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

        public static async Task CreateDialogFromCookie(string resumeJson)
        {
            dynamic resumeData = JsonConvert.DeserializeObject(resumeJson);

            string botId = resumeData.address.botId;
            string channelId = resumeData.address.channelId;
            string userId = resumeData.address.userId;
            string conversationId = resumeData.address.conversationId;
            string serviceUrl = resumeData.address.serviceUrl;
            string userName = resumeData.userName;
            bool isGroup = resumeData.isGroup;

            var resume = new ResumptionCookie(new Address(botId, channelId, userId, conversationId, serviceUrl), userName, isGroup, "en_GB");

            var messageactivity = (Activity)resume.GetMessage();
            var client = new ConnectorClient(new Uri(messageactivity.ServiceUrl));

            var reply = messageactivity.CreateReply();
            reply.Text = "Hey! I have more *exciting news*! Come back!";

            //await client.Conversations.ReplyToActivityAsync(reply);
            //TODO: Do I need to start a new dialog from here or can I just display the Card?
            var newConvo = await client.Conversations.CreateDirectConversationAsync(messageactivity.Recipient, messageactivity.From, reply);
            messageactivity.Conversation.Id = newConvo.Id;

        }

    }
}