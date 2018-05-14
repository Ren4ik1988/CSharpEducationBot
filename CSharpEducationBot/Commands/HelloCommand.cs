using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace CSharpEducationBot.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => "hello";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;            

            await client.SendTextMessageAsync(chatId, "Hello!", replyToMessageId: messageId);
        }

        internal override void CheckCallBack(string v, CallbackQueryEventArgs e)
        {
        }
    }
}
