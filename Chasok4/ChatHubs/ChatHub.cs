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
        public UnitOfWork uM = new UnitOfWork();
                
        public void Join(string roomName)
        {
            Groups.Add(Context.ConnectionId, roomName);            
        }

        public void Send(MyMessage message)
        {
            IEnumerable<AppUser> allUsers = uM.User.GetUsers();
            AppUser currentUser = uM.User.GetUserById(message.SenderId);
            Message newMessage = new Message();
            

            newMessage.Body = message.Msg;
            newMessage.CreateDate = DateTime.Now.ToLocalTime();
            newMessage.CreatorId = message.SenderId;
            uM.Message.AddMessage(newMessage);
             
            string b;
            foreach (AppUser receaverUser in allUsers)
            {
                b = receaverUser.Email;
                foreach (string a in message.SelectedUsers)
                    if (a==b)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        newUserMessage.MessageTo = newMessage;
                        newUserMessage.Receiver = receaverUser;
                        uM.UserMessage.AddUserMessage(newUserMessage);
                    }
            }
            uM.Save();
            

            Clients.Groups(message.SelectedUsers).addMessage(message.SenderName + ":\n" + message.Msg);
            Clients.Client(Context.ConnectionId).myMessage("My message:\n" + message.Msg);
        }

            private static List<string> users = new List<string>();
        public override Task OnConnected()
        {
            users.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        ////SignalR Verions 1 Signature
        //public override Task OnDisconnected()
        //{
        //    users.Remove(Context.ConnectionId);
        //    return base.OnDisconnected();
        //}

        //SignalR Version 2 Signature
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

    }
}
