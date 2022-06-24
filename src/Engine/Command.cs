
using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Engine
{


    /// <summary>
    /// Представление команд такие как /hello /send и многих других
    /// </summary>
    
    public interface Command{
        public void execute(out string outResult, string Arguments = null);
    }

   
}
