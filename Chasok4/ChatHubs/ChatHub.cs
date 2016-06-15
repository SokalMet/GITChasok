using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Chasok4.Models.Entities;
using Chasok4.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR.Hubs;

namespace Chasok4.ChatHubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        static List<AppUser> ChatUsers = new List<AppUser>();

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
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
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
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