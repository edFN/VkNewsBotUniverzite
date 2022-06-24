using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VkBot
{
    public class ConsoleLog
    {
        
        
        public static void ErrorMessage(string msg)
        {
            DateTime time = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"[ERROR] - ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(time+" - " +msg);
            
        }
        public static void Standart_Message(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            DateTime time = DateTime.Now;
            Console.Write($"[INFO] - ");
            Console.WriteLine(time + " - " + msg);
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void success_message(string msg)
        {
            DateTime time = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SUCCESS] -");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{time} - {msg}");
        }
    }
    class FileLog
    {
        
        public static void Standart_Message(string message)
        {
            
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/logs"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/logs");
            }
           
            if(!File.Exists(Directory.GetCurrentDirectory() + $"/logs/log{DateTime.Now.Day}.txt")){
                
                File.Create(Directory.GetCurrentDirectory() + $"/logs/log{DateTime.Now.Day}.txt");
            }
            File.WriteAllText(Directory.GetCurrentDirectory()+$"/logs/log{DateTime.Now.Day}.txt",$"[INFO] - {DateTime.Now} - {message}\n");
            
        }
        public static void ErrorMessage(string message)
        {
            try
            {
                var directory = Directory.GetCurrentDirectory() + "/logs";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(directory+$"/log{DateTime.Now.Day}.txt"))
                {
                    File.Create(directory + $"/log{DateTime.Now.Day}.txt");
                }
                using (System.IO.StreamWriter file = new StreamWriter(directory + $"/log{DateTime.Now.Day}.txt", true, Encoding.Default))
                {
                    file.WriteLine($"[ERROR] - {DateTime.Now} - {message}\n");
                }
               
            }
            catch (Exception e)
            {
                FileLog.ErrorMessage(e.Message);
            }
        }

    }


}
