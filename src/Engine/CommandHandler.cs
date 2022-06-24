using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Engine
{
    using CommandsDict = Dictionary<string, Command>;
    public class CommandHandler
    {
        
        const bool COMMAND_NOT_FOUND = false;

        public CommandsDict lCommands;
        private char prefix;

        
        public CommandHandler(char prefix = '/')
        {
            this.prefix = prefix;
            lCommands = new CommandsDict();
        }
        public CommandHandler(CommandsDict cmd,char prefix = '/') {
            this.lCommands = cmd;
            this.prefix = prefix;
        }
        public CommandHandler AddCommand(string key,Command value)
        {
            if (lCommands.ContainsKey(key)) throw new Exception("Use different key for command!");
            lCommands.Add(key, value);
            return this;
        }

        public bool HandleCommand(string command,out string outResult)
        {
            
            if (command.StartsWith(prefix))
            {
                if(TryFind(command,out outResult) == COMMAND_NOT_FOUND )
                {
                    return COMMAND_NOT_FOUND;
                }
                else
                {
                    return true;
                }
            }
            outResult = string.Empty;
            return false;
        }


        private static string noPrefixnArguments(string key)
        {
            return key.Split()[0].Substring(1);
        }

      
        private bool TryFind(string key,out string result)
        {
            string noPrefix = noPrefixnArguments(key); 
            if (lCommands.ContainsKey(noPrefix))
            {
                string argument = key.Substring(noPrefix.Length + 1);
                lCommands[noPrefix].execute(out result,argument);
                return true;
            }
            result = string.Empty;
            return COMMAND_NOT_FOUND;
        }
    }
}
