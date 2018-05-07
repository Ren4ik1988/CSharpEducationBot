using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CSharpEducationBot
{

    //При проверке соединия с сервером успешность соединения определяет возвращаемым объектом от сервера, 
    //если успешно, сервер возвращает объект, в данном случае это объекn User user (на самом деле json),
    //если сервер вернул не вернул ничего (null) - значит соединения нет

    static class ConnectionTest
    {
        internal async static Task<bool> Test(TelegramBotClient bot)
        {
            try
            {
                User user = await bot.GetMeAsync(); //пробуем подключиться к серверу телеграм,
                                                     //если успешно - сервер вернет объект User, иначе вернет - null

                Console.Title = user.Username; //установим в качестве заголовка окна консоли UserName нашего бота
                                               //необязательный пункт, но он улучшает вид окна и указывает, что ответ сервера успешно отбработан

                Program.log.Info("Связь с сервером телеграм... успешно.");
                Program.log.Info("Валидность токена подтверждена сервером.");
            }
            catch(Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Program.log.Info("Связь сервером телеграм... успешно.");
                Program.log.Info("Валидность токена не подтверждена сервером.");
                Program.log.Info("Соединение сброшено.");
                Program.log.Error(ex.Message);
                return false;
            }
            catch(Exception ex)
            {
                Program.log.Error(ex.Message);
                Program.log.Info("Связь с сервером телеграм... сервер недоступен.");
                return false;
            }
            return true;
        }
    }
}
