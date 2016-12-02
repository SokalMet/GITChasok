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
using Ninject;

namespace Chasok4.ChatHubs
{
    [Authorize]
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        IUnitOfWork uW;// = new UnitOfWork();// = new UnitOfWork();
        public ChatHub()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            uW = ninjectKernel.Get<IUnitOfWork>();
        }

        public void SaveToDb(string mess, string userId, string userName, List<string> selectedInUsers, DateTime date)
        {
            IEnumerable<AppUser> allUsers = uW.User.GetUsers();
            Message newMessage = new Message();
            AppUser currentUser = uW.User.GetUserById(userId);
            selectedInUsers.Add(currentUser.Email);
            newMessage.Body = mess;
            newMessage.CreateDate = date.ToLocalTime();
            newMessage.CreatorId = userId;
            uW.Message.AddMessage(newMessage);

            foreach (AppUser receiverUser in uW.User.GetUsers())
            {
                string b = receiverUser.Email;
                foreach (string a in selectedInUsers)
                    if (a == b)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        newUserMessage.Message = newMessage;
                        newUserMessage.Receiver = receiverUser;
                        uW.UserMessage.AddUserMessage(newUserMessage);
                        if (a == userName)
                            newUserMessage.ReadDate = date.ToLocalTime();
                    }
            }
            uW.Save();
        }

        public void MessageReadDate(DateTime readDate, string userId)
        {
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages().Where(x => x.ReceiverId == userId).ToList();
            foreach (var item in allUsersMessages)
            {
                if (item.ReadDate == null)
                {
                    item.ReadDate = readDate.ToLocalTime();
                    uW.Save();
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
            DateTime dateTimeNow = DateTime.Now;
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages().Where(x => x.ReceiverId == userId).ToList();
            IEnumerable<Message> allMessages = uW.Message.GetMessages().Where(x=>allUsersMessages.Select(y=>y.MessageId).Contains(x.Id)).ToList();
            
            foreach (var item in allMessages)
            {                
                AppUser unewUser = uW.User.GetUserById(item.CreatorId);
                    if (item.CreateDate.DayOfYear>=(dateTimeNow.DayOfYear-1))
                    Clients.Client(Context.ConnectionId).onConnected( new { mess=item.Body, creatoremail=unewUser.Email, createdate=item.CreateDate});
            }
            foreach (var item in allUsersMessages)
            {
                if (item.ReadDate == null)
                {
                    item.ReadDate = DateTime.Now.ToLocalTime();
                    uW.Save();
                }           
            }
        }

        public void OnConnectedAllHistory(string userId)
        {
            DateTime dateTimeNow = DateTime.Now;
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages().Where(x => x.ReceiverId == userId).ToList();
            IEnumerable<Message> allMessages = uW.Message.GetMessages().Where(x => allUsersMessages.Select(y => y.MessageId).Contains(x.Id)).ToList();

            foreach (var item in allMessages)
            {
                AppUser unewUser = uW.User.GetUserById(item.CreatorId);
                if (item.CreateDate.DayOfYear<(dateTimeNow.DayOfYear - 1))
                    Clients.Client(Context.ConnectionId).onConnectedAllHistory(new
                    {
                        mess = item.Body,
                        creatoremail = unewUser.Email,
                        createdate = item.CreateDate
                    });
            }
            foreach (var item in allUsersMessages)
            {
                if (item.ReadDate == null)
                {
                    item.ReadDate = DateTime.Now.ToLocalTime();
                    uW.Save();
                }
            }
        }
    }
}
