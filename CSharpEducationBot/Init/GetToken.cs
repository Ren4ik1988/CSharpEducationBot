using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using MihaZupan;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CSharpEducationBot
{
    static class GetToken //считывает токен из файла и проверяет его на валидность, 
                   //если удачно - создается экземляр бота, если неудачно - программа закрывается
    {
        static Properties.Settings param = new Properties.Settings();

        private const string filePath = "key.txt";

        internal static bool loadToken(ref TelegramBotClient bot)
        {
            try
            {
                param.Reload(); // читаем параметры из файла приложения, если этого не делать, то будут настройки по умолчанию (т.е. указанные при создании файла)


                // Определяем способ подключения к серверам
                if (param.usesocks5 && 
                   !String.IsNullOrEmpty(param.socksip)  && param.socksport != 0)
                {
                    var proxy = new HttpToSocks5Proxy(param.socksip, param.socksport);
                    proxy.ResolveHostnamesLocally = true;
                    bot = new TelegramBotClient(readToken(), proxy);
                }
                else
                    bot = new TelegramBotClient(readToken()); //инициализируем клиент телеграм бота

                bot.Timeout = new TimeSpan(0, 2, 0);
                
            }
            catch ( ArgumentException ex) // если файл найден, но токен имел неверный формат
            {
                Program.log.Info("Считывание токена... успешно");
                Program.log.Error(ex.Message);
                Program.log.Info($"Токен записан в некорректном формате или {filePath} не содержит данных.");
                return false;
            }
            catch (Exception ex) // если файл не найден
            {
                Program.log.Error(ex.Message);
                Program.log.Info($"Считывание токена невозможно: файл {filePath} не найден. Создайте файл {filePath}"+
                    " содержащий токен вашего бота и поместите его в папку Debug. Для создания бота и получения к нему токена - обращаться к боту @BotFather.");
                return false;
            }

            Program.log.Info("Считывание токена... успешно");
            Program.log.Info("Создание экземпляра клиента... успешно");
            return true;
        }
        private static string readToken() //считываем строку из файла
        {
            using (StreamReader read = new StreamReader(filePath))
                return read.ReadLine();
        }
    }
}
