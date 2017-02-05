﻿using System;
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
using Chasok4.Projections;

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

            foreach (AppUser receiverUser in allUsers)
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
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages(userId);
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
            Clients.AllExcept(userId).messageToAll(userName, mess, date.ToLocalTime().ToLongTimeString());         
            Clients.Groups(selectedInUsers).addMessage(userName, mess, date.ToLocalTime().ToLongTimeString());
            Clients.Client(Context.ConnectionId).myMessage(mess, "Me: ", date.ToLocalTime().ToLongTimeString());
        }

        public void OnConnected(string userId)
        {
            DateTime dateTimeNow = DateTime.Now;
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages(userId);
            IEnumerable<Message> allMessages = uW.Message.GetMessages(allUsersMessages);
            List<ProjectionUsers> allUsers = uW.User.UsersList();
            //IEnumerable<AppUser> allUsers = uW.User.GetUsers().Distinct();

            foreach (var item in allMessages)
            {
                //AppUser unewUser = uW.User.GetUserById(item.CreatorId);
                string email = allUsers.Where(x=>x.UserId == item.CreatorId).Select(y=>y.UserEmail).FirstOrDefault();
                    if (item.CreateDate.DayOfYear<=(dateTimeNow.DayOfYear-1))
                    Clients.Client(Context.ConnectionId).onConnected( new {
                        mess =item.Body, creatoremail= email, createdate=item.CreateDate});
            }
            foreach (var item in allUsersMessages.Where(el=>el.Message.CreateDate.DayOfYear<= (dateTimeNow.DayOfYear - 1)))
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
            IEnumerable<UserMessage> allUsersMessages = uW.UserMessage.GetUserMessages(userId);
            IEnumerable<Message> allMessages = uW.Message.GetMessages(allUsersMessages);
            IEnumerable<AppUser> allUsers = uW.User.GetUsers().Distinct();

            foreach (var item in allMessages)
            {
                string email = allUsers.Where(x => x.Id == item.CreatorId).Select(y => y.Email).FirstOrDefault();
                if (item.CreateDate.DayOfYear>(dateTimeNow.DayOfYear - 1))
                    Clients.Client(Context.ConnectionId).onConnectedAllHistory(new
                    {
                        mess = item.Body,
                        creatoremail = email,
                        createdate = item.CreateDate
                    });
            }
            foreach (var item in allUsersMessages.Where(el=>el.Message.CreateDate.DayOfYear>(dateTimeNow.DayOfYear - 1)))
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
