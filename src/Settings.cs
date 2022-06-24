using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace VkBot
{
    public class Settings
    {

        //Timer settings
        public int second { get; set; }
        public int minute { get; set; }
        //VK_API
        public string TOKEN { get; set; }
        //DataBase
        public string Server { get; set; }
        public string User  { get; set ;}
        public string DbName { get; set; }
        public string Port { get; set; }
        public string ConnectTimeout { get; set; }
        public string Password { get; set; }

    }
    interface SettingsGetter
    {
        Settings setSettings();
        void createSettings();
    }

    class JsonSettingsGetter : SettingsGetter
    {
       
        public void createSettings()
        {
            if (File.Exists(Directory.GetCurrentDirectory()+"/settings.json")) return;
            Settings settings = new Settings()
            {
                second = 60,
                minute = 1,
                TOKEN = "NULL",
                Server = "127.0.0.1",
                User = "root",
                DbName="newspaper",
                Port = "3306",
                ConnectTimeout = "5",
                Password = ""

            };
            

            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText("settings.json", json);
        }

        public Settings setSettings()
        {
            try
            {
                string json = File.ReadAllText("settings.json");
                Settings newSettings = JsonSerializer.Deserialize<Settings>(json);
                return newSettings;
            }catch(Exception e)
            {
#if DEBUG
                ConsoleLog.ErrorMessage(e.Message);
#endif
                return null;

            }
        }
    }









}
