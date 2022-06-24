using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Model.Attachments;
using System.Collections;
using System.Linq;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using System.Diagnostics.CodeAnalysis;
using VkBot.Models;

namespace VkBot.Engine
{
    

    public class BotEngine
    {
        public static VkApi _vkApi; // Core component
        public static VkNet.Model.GroupUpdate.MessageNew newMsg;

        private SpyWeb web;
           
        public CommandHandler _commandHandler { get; private set; }

        public static Models.NewsPaperContext context;
        public BotEngine(string token,Dictionary<string,Command> cmd = null)
        {
            //check this

            
            _vkApi = new VkApi();
            _vkApi.Authorize(new ApiAuthParams()
            {
                AccessToken = token
                
            });
 
            if(cmd != null)
            {
                _commandHandler = new CommandHandler(cmd);
            }
            else
            {
                _commandHandler = new CommandHandler();
            }
            context = new Models.NewsPaperContext();

            this.web = new SpyWeb(new List<Models.PageManager>
            {
                new Models.ItmoPageManager(),
                new Models.MsuPageManager(),
                new Models.SpbuPageManager()
            });
            web.UpdatedEvent += Database.onUpdated;


            ConsoleLog.success_message("Bot engine has started!");
            ProcessEvents();
        }

        private void ProcessEvents()
        {
            string result;
            
                while (true)    
                {
                try
                {
                    var pollServer = _vkApi.Groups.GetLongPollServer(203131641);
                    var poll = _vkApi.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {

                        Server = pollServer.Server,
                        Ts = pollServer.Ts,
                        Wait = 25,
                        Key = pollServer.Key

                    });
                    if (poll.Updates == null || poll.Updates.ToList().Count == 0) continue;
                    
                    foreach(var upd in poll.Updates) {
                        if (upd.Type == GroupUpdateType.MessageNew)
                        {
                            var msg = upd.MessageNew.Message.Text;
                            newMsg = upd.MessageNew;

                            if (_commandHandler.HandleCommand(msg, out result))
                            {
                                Engine.Funcx.BotFunc.Send(result, upd.MessageNew, ref _vkApi);
                                continue;
                            }
                            if(msg.ToLower() == "начать")
                            {

                                StringBuilder bd = new StringBuilder();
                                bd.AppendLine($"Пишите 'Главное' если хотите получить все важные предстоящие мероприятия");
                                bd.AppendLine($"Пишите 'Последние новости' если хотите получить все последние новости");
                                Funcx.BotFunc.Send(bd.ToString(), newMsg, ref _vkApi);

                            }
                            if (msg.ToLower() == "последние новости")
                            {
                                unglavnoe(upd.MessageNew);

                            }
                            else if (msg.ToLower() == "главное")
                            {
                                glavnoe(upd.MessageNew);
                            }
                        }
                    }
                }
                catch (Flurl.Http.FlurlHttpException e)
                {
                    FileLog.ErrorMessage("Connection Proble");
                    FileLog.ErrorMessage(e.Message);
                    // System.Environment.Exit(1);
                }
                catch (Exception e)
                {
                    FileLog.ErrorMessage("BotEngine");
                    FileLog.ErrorMessage(e.Message);

                    //  System.Environment.Exit(1);
                }
            }
                
            
           
        }

        private void unglavnoe(VkNet.Model.GroupUpdate.MessageNew p) 
        {
            try
            {
                var items = context.set.AsQueryable().Where(x => x.main == false).ToList();
                int i = 1;
                items.Sort(new SortByDate());
                
                if (items.Count > 0)
                {
                    StringBuilder bd = new StringBuilder();
                    for(i=0;i<items.Count && i < 10;i++)
                    {
                        
                        bd.AppendLine($"[{i+1}] {items[i].university}: {items[i].Title}\n");
                        
                    }

                    bd.AppendLine("Для подробностей пишите : /nmain [number]");
                    Funcx.BotFunc.Send(bd.ToString(), p, ref _vkApi);
                }
            }catch(Exception e)
            {
                FileLog.ErrorMessage(e.Message);
            }

        }
        private void glavnoe(VkNet.Model.GroupUpdate.MessageNew p)
        {
            try
            {
                // DateTime elapsed = DateTime.Now;
                var items = context.set.AsQueryable().Where(x => x.main == true).ToList();
                //DateTime newElapsed = DateTime.Now;
                //TimeSpan delta = newElapsed - elapsed;
                // ConsoleLog.Standart_Message("Time Passed: " + delta.TotalSeconds);
                int i = 1;
                if (items.Count > 0 && items != null)
                {
                    
                    StringBuilder bd = new StringBuilder();
                    
                    foreach (var item in items)
                    {
                        if (DateTime.Now <= item.date)
                        {
                            bd.AppendLine($"[{i}]{item.university} - {item.Title}  ✅");
                        }
                        else
                            bd.AppendLine($"[{i}]{item.university} - {item.Title}  🚫");
                        i++;
                    }
                    bd.AppendLine("Для подробностей пишите : /main [number]");
                    Funcx.BotFunc.Send(bd.ToString(), p, ref _vkApi);
                }
                else
                {
                    Funcx.BotFunc.Send("Пока ничего нет", p, ref _vkApi);

                }
            }catch(Exception e)
            {
                FileLog.ErrorMessage(e.Message);
            }

        }


    }
    class SortByDate : IComparer<Models.NewsPaperContext.NewsPaper>
    {
        public int Compare([AllowNull] NewsPaperContext.NewsPaper x, [AllowNull] NewsPaperContext.NewsPaper y)
        {
            if (x.date > y.date) return -1;
            else if (x.date < y.date) return 1;
            else return 0;
        }
    }
}
