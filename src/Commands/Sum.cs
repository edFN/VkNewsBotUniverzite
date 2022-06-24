using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Commands
{
    class Sum : Engine.Command
    {
        public Sum() { }
        public void execute(out string outResult,string Arguments = null)
        {
            if (string.IsNullOrEmpty(Arguments))
            {
                outResult = "Not enough arguments";
            }
            else
            {
                string[] Args = Arguments.Split(" ");
                double[] args = new double[Args.Length];
                int realSize = 0;
                int spaces = 0;
                for(int i =0; i < Args.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(Args[i]))
                    {
                        spaces += 1;
                        continue;
                    }
                    if(Double.TryParse(Args[i],out args[i-spaces]) == false)
                    {
                        //Console.WriteLine("Wrong Arguments");
                        continue;
                    }
                    realSize += 1;
                    
                }
                double sum = 0;
                for(int i = 0; i < realSize; i++)
                {
                    sum += args[i];
                }
                
                outResult = $"Sum is: {sum}";
            }
        }
    }
}
