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
        static UnitOfWork uM = new UnitOfWork();
        IEnumerable<AppUser> allUsers;

        public void SaveToDb(string mess, string userId, string userName, List<string> selectedInUsers, DateTime date)
        {
            allUsers = uM.User.GetUsers();
            Message newMessage = new Message();
            AppUser currentUser = uM.User.GetUserById(userId);
            selectedInUsers.Add(currentUser.Email);
            newMessage.Body = mess;
            newMessage.CreateDate = date;
            newMessage.CreatorId = userId;
            uM.Message.AddMessage(newMessage);

            foreach (AppUser receiverUser in allUsers)
            {
                string b = receiverUser.Email;
                foreach (string a in selectedInUsers)
                    if (a == b)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        newUserMessage.Message = newMessage;
                        newUserMessage.Receiver = receiverUser;
                        uM.UserMessage.AddUserMessage(newUserMessage);
                        if (a == userName)
                            newUserMessage.ReadDate = date;
                    }
            }
            uM.Save();
        }

        public void MessageReadDate(DateTime readDate, string userId)
        {
            IEnumerable<UserMessage> allUsersMessages = uM.UserMessage.GetUserMessages().Where(x => x.ReceiverId == userId).ToList();
            foreach (var item in allUsersMessages)
            {
                if (item.ReadDate == null)
                {
                    item.ReadDate = readDate;
                    uM.Save();
                }
            }
        }

        public void Join(string roomName)
        {
            Groups.Add(Context.ConnectionId, roomName);            
        }
               

        public void Send(string mess, string userId, string userName, List<string> selectedInUsers, DateTime date)
        {            
            Clients.Groups(selectedInUsers).addMessage(userName, mess, date.ToLocalTime().ToLongTimeString());
            Clients.Client(Context.ConnectionId).myMessage(mess, "Me: ", date.ToLocalTime().ToLongTimeString());
        }

        public void OnConnected(string userId)
        {   
            IEnumerable<UserMessage> allUsersMessages = uM.UserMessage.GetUserMessages().Where(x => x.ReceiverId == userId).ToList();
            IEnumerable<Message> allMessages = uM.Message.GetMessages().Where(x=>allUsersMessages.Select(y=>y.MessageId).Contains(x.Id)).ToList();
            foreach (var item in allMessages)
            {
                Clients.Client(Context.ConnectionId).onConnected( new { mess=item.Body, creatoremail=item.Creator.Email, createdate=item.CreateDate});
            }
            foreach (var item in allUsersMessages)
            {
                if (item.ReadDate == null)
                {
                    item.ReadDate = DateTime.Now.ToLocalTime();
                    uM.Save();
                }
            }
        }        
    }
}
