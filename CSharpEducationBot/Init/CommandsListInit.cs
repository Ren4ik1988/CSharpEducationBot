using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpEducationBot.Commands;


namespace CSharpEducationBot
{
    public static class CommandsListInit
    {
        internal static void loadCommands(ref List<Command> commandsList)
        {
            commandsList = new List<Command>();
            commandsList.Add(new HelloCommand());
        }
    }
}
