using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Args;
using System.Data.SQLite;
using CSharpEducationBot.Commands; // подключаем простанство имен, где лежат команды 

namespace CSharpEducationBot
{
    internal class Program
    {
        static internal Logger log = LogManager.GetCurrentClassLogger(); // логгер

        private static TelegramBotClient bot;

        public static mainEntities dbConn;

        // список команд
        private static List<Command> commandsList;


        static async Task Main(string[] args)
        {
            log.Info("Запуск консоли");

            //устанавливаем связь с базой данных
            dbConn = new mainEntities();
            dbConn.Courses.Add(new Cours
            {
                Name = "Курс какой-то непонятный",
                Url = "reference to course",
            });
            dbConn.SaveChanges();
            


            if (await initBot())
            {
                log.Info("Проверка завершена.");
                await startAsync(); //запуск опроса чатов и запуск метода закрытия консоли
                                
            }
            else
            {
                log.Info("Проверка завершилась с ошибками. Попробуйте перезапустить клиент.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            
            log.Info("Остановка консоли");
        }

        #region Опрос чата и его завершение
        private async static Task startAsync()
        {
            bot.OnMessage += Bot_OnMessage; // присваиваем событие
            bot.OnCallbackQuery += Bot_OnCallbackQuery;
            bot.StartReceiving(); //запуск опроса чата
            await Task.Run(stopBot);
        }

        
        private async static Task stopBot()
        {
            bool stop = false;
            while(!stop)
            {
                string adminCommand = Console.ReadLine();
                if(adminCommand.Equals("stop"))
                {
                    bot.StopReceiving();
                    stop = true;
                }
            }
            await Task.Delay(0);
        }
        #endregion

        private static void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string[] input = e.CallbackQuery.Data.Split('_');
            Command RunCommand = commandsList.Find(s => s.Name == input[0]);
            if (RunCommand != null)
            {
                RunCommand.CheckCallBack(input[1], e);
            }

        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Message msg = e.Message; // получаем сообщение

            // поиск класса у которого поле Name совпадает с входящей командой
            Command RunCommand = commandsList.Find(s => s.Name == msg.Text);
            if (RunCommand != null)
                RunCommand.Execute(msg, bot);

        }

        /*метод проверяет наличие токена, его валидность и доступность сервера, если в процессе проверки просиходит ошибка, управление 
        данному методу не возвращается - приложение выводит на экран логи ошибок и закрывается, именно поэтому метод всегда возвращает true */
        private async static Task<bool> initBot()
        {
            log.Info("Запуск проверки валидности токена и доступности сервера.");

            if (GetToken.loadToken(ref bot) && await ConnectionTest.Test(bot))
            {
                // загружаем список команд
                CommandsListInit.loadCommands(ref commandsList);

                return true;
            }
            return false;
        }
    }
}
