﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Args;
using CSharpEducationBot.Commands; // подключаем простанство имен, где лежат команды 

namespace CSharpEducationBot
{
    internal class Program
    {
        static internal Logger log = LogManager.GetCurrentClassLogger(); // логгер

        private static TelegramBotClient bot;

        // список команд
        private static List<Command> commandsList;


        static async Task Main(string[] args)
        {
            log.Info("Запуск консоли");

            if (await initBot())
            {
                log.Info("Проверка завершена.");
                await StartAsync();
                                
            }
            else
            {
                log.Info("Проверка завершилась с ошибками. Попробуйте перезапустить клиент.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            
            log.Info("Остановка консоли");
        }

        private async static Task StartAsync()
        {
            bot.OnMessage += Bot_OnMessage; // присваиваем событие
            bot.StartReceiving(); //запуск опроса чата
            await Task.Run(stopBot);
        }

        private static Task stopBot()
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
