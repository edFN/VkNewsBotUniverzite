using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VkBot.VkInteraction;
namespace VkBot.Commands
{
    class MainCommand : Engine.Command
    {

       

        public void execute(out string outResult, string Arguments = null)
        {
#if DEBUG
            ConsoleLog.Standart_Message($"In method MainCommand.execute => argument({Arguments})");
#endif
            outResult = string.Empty;
            try
            {

                var items = Engine.BotEngine.context.set.AsQueryable().Where(x => x.main == true).ToList();//.Select(x=>x.Title);
                StringBuilder builder = new StringBuilder();
                if (items != null || items.Count > 0)
                {
                    if (string.IsNullOrEmpty(Arguments))
                    {

     
                        
                        foreach (var item in items)
                        {
                            if (DateTime.Now < item.date)
                                builder.AppendLine(item.Title + " ✅");
                            else
                            {
                                builder.AppendLine(item.Title + " 🚫");
                            }
                            builder.AppendLine();
                        }
                        

                    }
                    else
                    {
                        var splited = Arguments.Split();
                        int intResult;
                        foreach (var el in splited)
                        {
                            if (Int32.TryParse(el, out intResult))
                            {
                                intResult = Math.Abs(intResult - 1);
                                if (intResult >= items.Count)
                                {
                                    continue;
                                }
                                else
                                {
                                    var element = items[intResult];
                                    if (DateTime.Now <= element.date)
                                    {
                                        builder.AppendLine($"[✅]\n{element.ToString()}");

                                        VkInteraction.MessageHelper.SendWithImage(ConverterNewsPaper.ConvertToManager(element)
                                            , element.ToString());
                                    }
                                    else
                                    {
                                        builder.AppendLine($"[🚫]{element.ToString()}");
                                        VkInteraction.MessageHelper.SendWithImage(ConverterNewsPaper.ConvertToManager(element)
                                            , element.ToString());
                                    }
                                }
                            }
                        }
                    }
                    outResult = builder.ToString();
                }
                else
                {
                    outResult = "Empty";
                }
            }catch(Exception e)
            {
                FileLog.ErrorMessage(e.Message);
            }

        }
    }
}
