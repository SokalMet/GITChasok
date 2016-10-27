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
            var allUsers = uM.User.GetUsers();
            AppUser currentUser = uM.User.GetUserById(message.SenderId);
            Message newMessage = new Message();

            newMessage.Body = message.Msg;
            newMessage.CreateDate = DateTime.Now.ToLocalTime();
            newMessage.Creator = currentUser;

            currentUser.MessageToSend = newMessage;


            foreach (AppUser u in allUsers)
            {
                if (message.SelectedUsers.Equals(u.UserName))
                {
                    u.IncomeMessages.Add(newMessage);
                }
            }

            //Saving all to database
            if (newMessage != null)
            {
                uM.Message.AddMessage(newMessage);
                uM.User.UpdateUser(currentUser);
                uM.Save();
            }

            Clients.Groups(message.SelectedUsers).addMessage(message.SenderName + ": " + message.Msg);
            Clients.Client(Context.ConnectionId).myMessage("My message: " + message.Msg );
        } 
    }
}