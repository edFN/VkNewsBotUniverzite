using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VkBot.Engine;

namespace VkBot
{



    class Program
    {

        public static Settings settings;
        public static void Init()
        {
            JsonSettingsGetter getter = new JsonSettingsGetter();
            getter.createSettings();
            settings = getter.setSettings();
        }

        public static void Main(string[] args)

        {
            //need to be in text
            Init();
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
          

            string TOKEN = settings.TOKEN;

            if(TOKEN == "NULL")
            {
                ConsoleLog.ErrorMessage("Wrong Token");
                Thread.Sleep(1000);
                Process.GetCurrentProcess().Kill();
            }
            
           
            Dictionary<string, Command> cmds = new Dictionary<string, Command>
            {
                {"hello",new Commands.HelloCommand() },
                {"sum", new Commands.Sum() },
                {"main",new Commands.MainCommand() },
                {"nmain",new Commands.NonMainCommand() }
            };

            //BotEngine engine = new BotEngine(TOKEN, cmds);

            BotEngine engine = new BotEngine(TOKEN,cmds);
           
          
        }

    }

            


    
        
}

