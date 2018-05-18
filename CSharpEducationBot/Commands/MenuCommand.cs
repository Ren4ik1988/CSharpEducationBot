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
        string priorMenu = "";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            client.OnCallbackQuery += (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                ProcessingCall(message, client,ev.CallbackQuery.Data);
                client.AnswerCallbackQueryAsync(ev.CallbackQuery.Id);
            };
        }

        /// <summary>
        /// Процедура нужна для возможности хождения по меню в обратном направлении
        /// </summary>
        /// <param name="message"></param>
        /// <param name="client"></param>
        private async void ProcessingCall(Message inMessage, TelegramBotClient inClient, string inCall)
        {
            var chatId = inMessage.Chat.Id;
            var messageId = inMessage.MessageId;

            #region Главное меню
            string MainMenuText = "Привет! Данный бот поможет в поиске ресусов по С#. У нас есть видео и книги. Если есть чем поделиться, мы быдем рады.";
            var mainMenu = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Видео уроки", "video"),
                    InlineKeyboardButton.WithCallbackData("Книги", "books"),
                    InlineKeyboardButton.WithCallbackData("Поделиться знаниями", "share")
                }
            });
            
            #endregion
            

            
            switch (inCall)
            {
                case "MainMenu":
                    {
                        await inClient.SendTextMessageAsync(chatId, MainMenuText, replyMarkup: mainMenu);                        
                        break;
                    }

                case "video":
                    {
                        var ListMenuVideo = new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("1", "video1"),
                                InlineKeyboardButton.WithCallbackData("2", "video2"),
                                InlineKeyboardButton.WithCallbackData("3", "video3"),
                                InlineKeyboardButton.WithCallbackData("4", "video4"),
                                InlineKeyboardButton.WithCallbackData("5", "video5"),
                                InlineKeyboardButton.WithCallbackData("6", "video6")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("<-", "backList"),
                                InlineKeyboardButton.WithCallbackData("->", "forward")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Поиск", "search"),
                                InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                            }
                        });

                        priorMenu = "MainMenu";

                        // список видео и ссылки на него получаем из БД
                        await inClient.EditMessageTextAsync(chatId, messageId, "список видео", replyMarkup: ListMenuVideo);
                        break;
                    }

                case "books":
                    {
                        var ListMenuBook = new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("1", "book1"),
                                InlineKeyboardButton.WithCallbackData("2", "book2"),
                                InlineKeyboardButton.WithCallbackData("3", "book3"),
                                InlineKeyboardButton.WithCallbackData("4", "book4"),
                                InlineKeyboardButton.WithCallbackData("5", "book5"),
                                InlineKeyboardButton.WithCallbackData("6", "book6")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("<-", "backList"),
                                InlineKeyboardButton.WithCallbackData("->", "forward")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Поиск", "search"),
                                InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                            }
                        });
                        
                        priorMenu = "MainMenu";

                        await inClient.EditMessageTextAsync(chatId, messageId, "список книг", replyMarkup: ListMenuBook);
                        break;
                    }

                case "share":
                    {
                        priorMenu = "MainMenu";

                        await inClient.EditMessageTextAsync(chatId, messageId, "поделиться данными", replyMarkup: mainMenu);
                        break;
                    }

                case "video1":
                case "video2":
                case "video3":
                case "video4":
                case "video5":
                case "video6":
                    {
                        priorMenu = "video";
                        var discriptionMenu = new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Рейтинг", "rang"),
                                InlineKeyboardButton.WithCallbackData("Скачать", "download")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back"),
                                InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                            }
                        });
                        await inClient.EditMessageTextAsync(chatId, messageId, "Полное описание выбранных вещей", replyMarkup: discriptionMenu);

                        break;
                    }
                case "book1":
                case "book2":
                case "book3":
                case "book4":
                case "book5":
                case "book6":
                    {
                        priorMenu = "books";
                        var discriptionMenu = new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Рейтинг", "rang"),
                                InlineKeyboardButton.WithCallbackData("Скачать", "download")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back"),
                                InlineKeyboardButton.WithCallbackData("В главное меню", "MainMenu")
                            }
                        });
                        await inClient.EditMessageTextAsync(chatId, messageId, "Полное описание выбранных вещей", replyMarkup: discriptionMenu);

                        break;
                    }
                case "rang":
                    {
                        priorMenu = "1"; //выставляется в зависимости от

                        var rangMenu = new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("0", "rang0"),
                                InlineKeyboardButton.WithCallbackData("1", "rang1"),
                                InlineKeyboardButton.WithCallbackData("2", "rang2"),
                                InlineKeyboardButton.WithCallbackData("3", "rang3"),
                                InlineKeyboardButton.WithCallbackData("4", "rang4"),
                                InlineKeyboardButton.WithCallbackData("5", "rang5")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back")
                            }
                        });

                        await inClient.EditMessageTextAsync(chatId, messageId, "Оцените выбранный Вами ресурс", replyMarkup: rangMenu);

                        break;
                    }
                case "rang0":
                case "rang1":
                case "rang2":
                case "rang3":
                case "rang4":
                case "rang5":
                    {
                        break;
                    }
                case "back":
                    {
                        ProcessingCall(inMessage,inClient, priorMenu);
                        break;
                    }
            }
            
        }
    }
}
