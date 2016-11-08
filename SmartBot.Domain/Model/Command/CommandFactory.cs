using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBot.Domain.Model.Command
{
    public class CommandFactory
    {
        private static readonly IReadOnlyDictionary<string, BotCommand> InstanceList;


        static CommandFactory()
        {
            InstanceList = GetInstance();
        }

        private static IReadOnlyDictionary<string, BotCommand> GetInstance()
        {
            return new ReadOnlyDictionary<string, BotCommand>(
                    typeof(CommandFactory).Assembly.GetTypes()
                    .Where(x => (x.IsClass && !x.IsAbstract) && x.IsSubclassOf(typeof(BotCommand)))
                    .Select(x => (BotCommand)Activator.CreateInstance(x))
                    .ToDictionary(x => x.CommandText)
                );
        }

        public static BotCommand Create(string text)
        {
            var input        = text.Split(':');
            var commandText  = input.FirstOrDefault();
            var argumentText = input.LastOrDefault();

            var command = InstanceList.ContainsKey(commandText)
                ? InstanceList[commandText]
                : InstanceList["notexists"];

            command.Arguments = argumentText?.Split(' ');

            return command;
        }
    }
}
