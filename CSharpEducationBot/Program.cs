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

namespace CSharpEducationBot
{
    internal class Program
    {
        static internal Logger log = LogManager.GetCurrentClassLogger(); // логгер

        private static TelegramBotClient bot;

        static void Main(string[] args)
        {
            log.Info("Запуск консоли");

            if (initBot().Result)
            {
                log.Info("Проверка завершена.");

                bot.OnMessage += Bot_OnMessage; // присваиваем событие

                bot.StartReceiving(); //запуск опроса чата                
            }
            else
            {
                log.Info("Проверка завершилась с ошибками. Попробуйте перезапустить клиент.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.ReadLine();
            log.Info("Остановка консоли");
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Message msg = e.Message; // получаем сообщение
            
            await bot.SendTextMessageAsync(msg.Chat.Id, msg.Text);
        }

        //метод проверяет наличие токена, его валидность и доступность сервера, если в процессе проверки просиходит ошибка, управление 
        //данному методу не возвращается - приложение выводит на экран логи ошибок и закрывается, именно поэтому метод всегда возвращает true
        private async static Task<bool> initBot()
        {
            log.Info("Запуск проверки валидности токена и доступности сервера.");

            if (GetToken.loadToken(ref bot) && await ConnectionTest.Test(bot))
                return true;
            return false;
        }
    }
}
