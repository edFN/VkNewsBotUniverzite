using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VkBot.VkInteraction;
namespace VkBot.Commands
{
    class NonMainCommand : Engine.Command
    {


        public void execute(out string outResult, string Arguments = null)
        {
#if DEBUG
            ConsoleLog.Standart_Message($"In method NonMain.execute => argument({Arguments})");
#endif
            outResult = string.Empty;
            try
            {

                var items = Engine.BotEngine.context.set.AsQueryable().Where(x => x.main == false).ToList();//.Select(x=>x.Title);
                StringBuilder builder = new StringBuilder();
                if (items != null || items.Count > 0)
                {
                    if (string.IsNullOrEmpty(Arguments))
                    {
                        int i = 1;
                        foreach (var item in items)
                        {
                            if (DateTime.Now < item.date)
                                builder.AppendLine($"[{i}] {item.Title}  ✅");
                            else
                            {
                                builder.AppendLine($"[{i}] {item.Title}  🚫");
                            }
                            i++;
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
                                        builder.AppendLine($"[🚫]\n{element.ToString()}");
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

            }
            catch (Exception e)
            {
                FileLog.ErrorMessage(e.Message);
            }


        }
    }
}
