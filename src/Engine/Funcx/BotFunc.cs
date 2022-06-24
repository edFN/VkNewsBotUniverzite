using System;
using System.Collections.Generic;
using System.Text;
using VkNet.Model.GroupUpdate;

namespace VkBot.Engine.Funcx
{
    public static class BotFunc
    {

        //for simple sending
        public static void Send(string message,MessageNew msg, ref VkNet.VkApi _api)
        {
            try
            {
                _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
                {
                    Message = message,
                    UserId = msg.Message.FromId,
                    RandomId = (int?)msg.Message.Id
                });
            }catch(Exception e)
            {
                ConsoleLog.ErrorMessage(e.Message);
            }
        }
    }
}
