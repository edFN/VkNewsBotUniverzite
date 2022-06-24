using System;
using System.Collections.Generic;
using System.Text;
using VkBot.Models;
using System.Timers;
using System.Threading.Tasks;
using System.Linq;

namespace VkBot.Engine
{
    public class OnUpdatedArgs {
        public string html { get; set; }
        public PageManager manager { get; set; }


    }

   
    public class Database
    {


        private static string oldString = "";
        public async  static void onUpdated(object o, OnUpdatedArgs e)
        {
            try
            {
                using (NewsPaperContext db = new NewsPaperContext())
                {
                    var parsed = e.manager.getParser().Parse(e.html);
                    var element = db.set.AsQueryable().Where(x => x.id == parsed.id).FirstOrDefault();
                    if (oldString != e.html && element == null)
                    {
                        oldString = e.html;

                        //    db.set.Add(parsed);
                        //VkInteraction.MessageHelper.SendAllWithImage(e.manager, parsed.ToString());
                          //  await  db.SaveChangesAsync();
                        VkInteraction.MessageHelper.SendWithImage(e.manager, parsed.ToString());
                    }
                    
                }
            }catch(Exception exception)
            {

                FileLog.ErrorMessage("SpyWeb method onUpdated");
                FileLog.ErrorMessage(exception.Message);


              //  System.Environment.Exit(1);
            }

#if DEBUG
            ConsoleLog.Standart_Message("Method Database.onUpdated has done his work\n");       
#endif
        }
        
    }

    public class SpyWeb
    {
        private static Timer timer;
        private List<Models.PageManager> managers;
        private Dictionary<PageManager, string> req;
        public event EventHandler<OnUpdatedArgs> UpdatedEvent;

        public SpyWeb(List<PageManager> managers)
        {
            this.managers = managers;
            this.req = new Dictionary<PageManager, string>();
            foreach(var keys in managers)
            {
                req.Add(keys, "0");
            }

          

            timer = new Timer(16000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Manage();
        }



        private async void Manage()
        {
            foreach(var manager in managers)
            {
                try
                {
                    var responseMessage = await manager.getMsg(manager.getPage().getUrl());
                    if (responseMessage == string.Empty) return;
                    if (req[manager].ToString() != responseMessage)
                    {
                        req[manager] = responseMessage;
                        OnUpdatedArgs args = new OnUpdatedArgs();
                        args.html = responseMessage;
                        args.manager = manager;
                        onUpdated(args);
                        
                    }
                }catch(Exception e)
                {

                    FileLog.ErrorMessage("in SPYWEB.cs method: manage()");
                    FileLog.ErrorMessage(e.Message);

                   // System.Environment.Exit(1);
                }
            }
        }
        protected virtual void onUpdated(OnUpdatedArgs args)
        {
            EventHandler<OnUpdatedArgs> handler = UpdatedEvent;
            if(handler != null)
            {
                handler(this, args);
            }
        }
        

    }
}
