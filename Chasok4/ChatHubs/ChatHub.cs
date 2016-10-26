using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Chasok4.Models.Entities;
using Chasok4.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR.Hubs;
using Chasok4.Models;
using System.Threading.Tasks;
using Chasok4.DAL;
using System.Data.Entity;

namespace Chasok4.ChatHubs
{
    [Authorize]
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        UnitOfWork uM = new UnitOfWork();
        static List<AppUser> ChatUsers = new List<AppUser>();
        ConversationRoom newConversationRoom = new ConversationRoom();

        public void Join(string roomName)
        {
            Groups.Add(Context.ConnectionId, roomName);
        }

        public void Send(MyMessage message)
        {
            //Clients.All.addMessage(message.Msg);

            Message newMessage = new Message();
            UserMessage newUserMessage = new UserMessage();
            
            
            newMessage.Body = message.Msg;
            newMessage.Friends = message.SelectedUsers;
            newUserMessage.UserSendId = message.SenderId;
            //newUserMessage.UserReceiveId = message. SenderId;
            newUserMessage.DataTimeSend = DateTime.Now.ToLocalTime();
            newUserMessage.DataTimeRead = DateTime.Now.ToLocalTime();
            newUserMessage.Message = newMessage;


            //Saving all to database
            //if (newMessage != null)
            //{
            //    uM.Message.AddMessage(newMessage);
            //    uM.UserMessage.AddUserMessage(newUserMessage);
            //    uM.Save();
            //}
           
            Clients.Groups(message.SelectedUsers).addMessage(message.SenderName +": "+message.Msg);
            Clients.Client(Context.ConnectionId).myMessage("My message: " + message.Msg);
        }       

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (!ChatUsers.Any(x => x.Id == id))
            {
                ChatUsers.Add(new AppUser { Id = id, UserName = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, ChatUsers);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        public override Task OnDisconnected(bool stopCalled)
        {
            var item = ChatUsers.FirstOrDefault(x => x.Id == Context.ConnectionId);
            if (item != null)
            {
                ChatUsers.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}