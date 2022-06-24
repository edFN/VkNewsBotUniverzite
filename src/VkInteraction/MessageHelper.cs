using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Attachments;

using VkNet.Model.Keyboard;

namespace VkBot.VkInteraction
{
    class MessageHelper
    {
        private VkApi _api;
        private ulong GroupId;

        public  MessageHelper(VkApi _api,ulong groupId)
        {
            if (!(_api.IsAuthorized)) throw new Exception("VkApi isnt Authorized");
            this._api = _api;
            this.GroupId = groupId;
        }

        public void SendMessageAll(string message)
        {
            var communityUsers = _api.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
            {
                GroupId = this.GroupId.ToString(),
            });

            Random rnd = new Random();
            foreach(var user in communityUsers)
            {
                _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams() {
                    RandomId = (int?)(user.Id+10000),
                    UserId = user.Id,
                    Message = message
                });

            }
        }
        public void SendMessage(string message, VkNet.Model.GroupUpdate.MessageNew p)
        {  
            _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
            {
                Message = message,
                UserId = p.Message.FromId,
                RandomId = (int?)p.Message.Id
            });
        }


        public async void SendKeyboard()
        {
            KeyboardBuilder builder = new KeyboardBuilder();
            builder.AddButton("Главное", "Вывести главные новости",KeyboardButtonColor.Negative);
            builder.AddButton("Последние новости", "Получить обычные новости", KeyboardButtonColor.Positive);
            MessageKeyboard kw = builder.Build();

            var communityUsers = _api.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
            {
                GroupId = this.GroupId.ToString(),
            });

            Random rnd = new Random();
            foreach (var user in communityUsers)
            {
                _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
                {
                    Keyboard = kw,
                    RandomId = (int?)user.Id+100000,    
                    UserId = user.Id,
                    Message = "text"
                });

            }

        }
        public static async void SendWithImage(Models.PageManager manager, string message)
        {
            var getMessageNew = Engine.BotEngine.newMsg;
            if (getMessageNew == null) return;

            List<MediaAttachment> attach = new List<MediaAttachment>();


            attach.Add(new Note()
            {

                Title = "Ya Zaebalsa",
                Text = "DIE TWICE",
                OwnerId = Engine.BotEngine._vkApi.UserId.Value,
                Id = new Random().Next()



            });
            var uploadServer = Engine.BotEngine._vkApi.Photo.GetMessagesUploadServer((long)getMessageNew.Message.FromId);
            var wc = new WebClient();
            var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, manager.getPage().getImage()));
            var photo = Engine.BotEngine._vkApi.Photo.SaveMessagesPhoto(result);

            

            if (Engine.BotEngine._vkApi.IsAuthorized)
            {
                Random rand = new Random();
                Engine.BotEngine._vkApi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
                {
                    //   RandomId = (int?)getMessageNew.Message.Id,
                    //   UserId = getMessageNew.Message.FromId,

                   
                    RandomId = (int?) rand.Next(),
                    UserId = 193960317,
                    Attachments = photo,
                    Message = message

                });
            }
        }
        public static async void SendAllWithImage(Models.PageManager manager, string message)
        {
           
            if (Engine.BotEngine._vkApi.IsAuthorized)
            {
                var communityUsers = Engine.BotEngine._vkApi.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
                {
                    GroupId = "203131641",
                });
                


                foreach (var user in communityUsers)
                {
                    try
                    {
                        
                        var uploadServer = Engine.BotEngine._vkApi.Photo.GetMessagesUploadServer((long)user.Id);
                        var wc = new WebClient();
                        var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, manager.getPage().getImage()));
                        var photo = Engine.BotEngine._vkApi.Photo.SaveMessagesPhoto(result);
                        Engine.BotEngine._vkApi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
                        {
                            RandomId = (int?)user.Id + (new Random().Next()),
                            UserId = user.Id,
                            Attachments = photo,
                            Message = message

                        });
                    }
                    catch(WebException e)
                    {
                        ConsoleLog.ErrorMessage(e.Message);
                        FileLog.ErrorMessage(e.Message);
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        ConsoleLog.ErrorMessage(e.Message);
#else
                        FileLog.ErrorMessage(e.Message);
#endif
                    }

                }
               
            }
        }





    }
}
