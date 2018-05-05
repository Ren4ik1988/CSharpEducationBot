using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace CSharpEducationBot
{
    class Program
    {
        static Logger log = LogManager.GetCurrentClassLogger(); // логгер


        static void Main(string[] args)
        {
            log.Info("Запуск консоли");

            log.Info("Остановка консоли");
            Console.ReadLine();
        }
    }
}
