using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Commands
{

    //Implementing /hello command
    class HelloCommand : Engine.Command
    {
        private string name;

        public HelloCommand()
        {
            name = string.Empty;
        }
        public HelloCommand(string name)
        {
            this.name = name;
        }

        public void execute(out string outResult,string Arguments = null)
        {
            if (string.IsNullOrEmpty(Arguments))
            {
                outResult = "Hello World";
            }
            else
            {
                string[] args = Arguments.Split();
                int move = 0;
                for (int i = 0; i < args.Length; i++) {
                    if (string.IsNullOrWhiteSpace(args[i])) move += 1;
                    else{
                        break;
                    }
                }
                outResult = "Flower bloom to " + args[move];
                


            }
        }

    }
}
