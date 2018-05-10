using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace CSharpEducationBot.Commands
{
    class StartCommand : Command
    {
        public override string Name => "/start";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            // делаем клавиатуру
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new [] // first row
                {
                    InlineKeyboardButton.WithCallbackData("1.1", "callback1.1"),
                    InlineKeyboardButton.WithCallbackData("1.2", "callback1.2"),
                },
                new [] // second row
                {
                    InlineKeyboardButton.WithCallbackData("2.1", "callback2.1"),
                    InlineKeyboardButton.WithCallbackData("2.2", "callback2.2"),
                }
            });


            await client.SendTextMessageAsync(chatId, "Это сообщение пробное", replyMarkup:  inlineKeyboard);


            client.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                var msg = ev.CallbackQuery.Message;

                
                if (ev.CallbackQuery.Data == "callback1.1")
                {
                    await client.EditMessageTextAsync(chatId,messageId,"блабла");
                    await client.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, "You hav choosen " + ev.CallbackQuery.Data, true);
                }
                else
                if (ev.CallbackQuery.Data == "callback2.2")
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "тест", replyToMessageId: msg.MessageId);
                    await client.AnswerCallbackQueryAsync(ev.CallbackQuery.Id); // отсылаем пустое, чтобы убрать "частики" на кнопке
                }
            };
        }
    }
}
