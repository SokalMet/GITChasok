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
        static List<AppUser> ChatUsers = new List<AppUser>();
        static UnitOfWork uM = new UnitOfWork();
        IEnumerable<AppUser> allUsers = uM.User.GetUsers();
        
        public void SaveToDb(MyMessage message)
        {
            Message newMessage = new Message();
            AppUser currentUser = uM.User.GetUserById(message.SenderId);

            newMessage.Body = message.Msg;
            newMessage.CreateDate = DateTime.Now.ToLocalTime();
            newMessage.CreatorId = message.SenderId;
            uM.Message.AddMessage(newMessage);

            foreach (AppUser receaverUser in allUsers)
            {
                string b = receaverUser.Email;
                foreach (string a in message.SelectedUsers)
                    if (a == b)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        newUserMessage.MessageTo = newMessage;
                        newUserMessage.Receiver = receaverUser;
                        uM.UserMessage.AddUserMessage(newUserMessage);
                    }
            }
            uM.Save();
        }
        public void Join(string roomName)
        {
            Groups.Add(Context.ConnectionId, roomName);            
        }

        public void Send(MyMessage message)
        {
            Clients.Groups(message.SelectedUsers).addMessage(message.SenderName + ":\n" + message.Msg);
            Clients.Client(Context.ConnectionId).myMessage("Me:\n" + message.Msg);            
        }


            private static List<string> users = new List<string>();
        public void Connect(string userId, string userName)
        {
            UserMessage userMessage = new UserMessage();
                userMessage = uM.UserMessage.GetUserMessages().SingleOrDefault(u=>u.ReceiverId==userId);

            var id = Context.ConnectionId;

            if (!ChatUsers.Any(x => x.Id == id))
            {
                ChatUsers.Add(new AppUser { Id = id, UserName = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, ChatUsers);
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
