using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BargainBot.Bot
{
    public class CreateDialog
    {

        public async Task CreateDialogFromMessage(IAwaitable<IMessageActivity> result, string messageText)
        {
            var incMessage = await result;

            var connector = new ConnectorClient(new Uri(incMessage.ServiceUrl));
            var botAccount = new ChannelAccount(incMessage.Recipient.Id, incMessage.Recipient.Name);
            var userAccount = new ChannelAccount(incMessage.From.Id, incMessage.From.Name);
            var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);
            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId.Id);
            message.Text = messageText;
            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);

        }
    }
}