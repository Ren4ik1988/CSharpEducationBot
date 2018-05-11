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
    class MenuCommand :Command
    {
        public override string Name => "/start";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var mainMenu = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Видео уроки", "video"),
                    InlineKeyboardButton.WithCallbackData("Книги", "books"),
                    InlineKeyboardButton.WithCallbackData("Поиск по всем ресурсам", "search")
                }
            });

           

            var mainBooksMenu = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Видео уроки", "video"),
                    InlineKeyboardButton.WithCallbackData("Книги", "books")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                }
            });

            string MainText = "Привет! Данный бот поможет в поиске ресусов по С#. У нас есть видео и книги. Если ты пока не знаешь что выбрать, то воспользуйся общим поиском по разделам.";
            await client.SendTextMessageAsync(chatId, MainText, replyMarkup: mainMenu);


            client.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                var callBack = ev.CallbackQuery;
                var msg = callBack.Message;

                switch (callBack.Data)
                {
                    case "video":
                        {
                            var mainVideoMenu = new InlineKeyboardMarkup(new[]
                            {
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData("Видео уроки", "video"),
                                    InlineKeyboardButton.WithCallbackData("Книги", "books")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                                }
                            });
                            // список видео и ссылки на него получаем из БД
                            await client.EditMessageTextAsync(chatId, messageId, "список видео", replyMarkup: mainVideoMenu);
                            await client.AnswerCallbackQueryAsync(callBack.Id);
                            break;
                        }
                    case "books":
                        {
                            await client.EditMessageTextAsync(chatId, messageId, "список книг", replyMarkup: mainBooksMenu);
                            await client.AnswerCallbackQueryAsync(callBack.Id);
                            break;
                        }
                    case "search":
                        {
                            await client.EditMessageTextAsync(chatId, messageId, "список книг", replyMarkup: mainMenu);
                            await client.AnswerCallbackQueryAsync(callBack.Id);
                            break;
                        }
                    case "MainMenu":
                        {
                            await client.SendTextMessageAsync(chatId, MainText, replyMarkup: mainMenu);
                            await client.AnswerCallbackQueryAsync(callBack.Id);
                            break;
                        }
                }
            };
        }
    }
}
