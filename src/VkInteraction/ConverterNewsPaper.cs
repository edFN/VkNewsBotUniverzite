using System;
using System.Collections.Generic;
using System.Text;
using VkBot.Models;
namespace VkBot.VkInteraction
{
    class ConverterNewsPaper
    {
        public static Models.PageManager ConvertToManager(NewsPaperContext.NewsPaper paper)
        {
            Models.PageManager man;
            if (paper.university == "ИТМО")
            {
                man = new ItmoPageManager();
            }
            else if (paper.university == "СПБГУ")
            {
                man = new SpbuPageManager();
            }
            else if (paper.university == "МГУ")
            {
                man = new MsuPageManager();
            }
            else
            {
                man = null;
                throw new Exception("Error");
            }
            return man;
        }
    }
}
