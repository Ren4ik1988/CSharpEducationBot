using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CSharpEducationBot
{
    static class GetToken //считывает токен из файла и проверяет его на валидность, 
                   //если удачно - создается экземляр бота, если неудачно - программа закрывается
    {
        private const string filePath = "key.txt";

        internal static bool loadToken(ref TelegramBotClient bot)
        {
            try
            {
                bot = new TelegramBotClient(readToken()); //инициализируем клиент телеграм бота
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
