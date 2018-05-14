using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Data.SQLite.EF6;

namespace CSharpEducationBot
{
    class StartCommand : Commands.Command
    {
        public override string Name => "/start";
        TelegramBotClient client;
        public async override void Execute(Message message, TelegramBotClient client)
        {
            this.client = client;
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var button1 = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton();
            button1.Text = "Книги";
            button1.CallbackData = "/start_books";

            var button2 = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton();
            button2.Text = "Курсы";
            button2.CallbackData = "/start_kurs";

            var buttpons = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
            {
                new []{ button1, button2 }

            };

            var reply = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(buttpons);
            await client.SendTextMessageAsync(chatId, "Выберите интересующую категорию: ", Telegram.Bot.Types.Enums.ParseMode.Default, replyMarkup: reply);
        }
        

        private async 
        Task
        sendKurs(CallbackQueryEventArgs e)
        {
            using (Program.dbConn = new mainEntities())
            {
                var cours = Program.dbConn.Courses;
                foreach(var c in cours)
                {
                    if(c.Id == 1)
                    {
                        string t = $"Курс {c.Name} доступен для скачивания по ссылке: {c.Url}";
                        await client.SendTextMessageAsync(chatId: e.CallbackQuery.Message.Chat.Id, t);
                    }
                }
                
            }
        }

        private async 
        Task
        sendBook(CallbackQueryEventArgs e)
        {
            using (Program.dbConn = new mainEntities())
            {
                var books = Program.dbConn.Books;
                foreach (var c in books)
                {
                    if (c.Id == 1)
                    {
                        string t = $"Курс {c.Name} доступен для скачивания по ссылке: {c.Url}";
                        await client.SendTextMessageAsync(chatId: e.CallbackQuery.Message.Chat.Id, t);
                    }
                }

            }
        }

        internal override async void CheckCallBack(string v, CallbackQueryEventArgs e)
        {
            switch (v)
            {
                case "books": await sendBook(e); break;
                case "kurs": await sendKurs(e); break;
                default: break;
            }
        }
    }
}
