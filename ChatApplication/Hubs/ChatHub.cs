﻿using ChatApplication.Data;
using ChatApplication.Helpers;
using ChatApplication.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub:Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ChatHub(ApplicationDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task SendMessage(string receiverId, string message)
        {
            var NowDate= DateTime.Now;
            var date=NowDate.ToShortDateString();
            var time=NowDate.ToShortTimeString();

            string senderId=_currentUserService.UserId;

            var messageToAdd = new Message()
            {
                Text = message,
                Date = NowDate,
                SenderId = senderId,
                ReceiverId = receiverId,
            };
            await _context.AddAsync(messageToAdd);
            await _context.SaveChangesAsync();
            List<string>users = new List<string>()
            {
                senderId,receiverId
            };
            await Clients.Users(users).SendAsync("ReceiveMessage", message, date, time, senderId);
        }
    }
}
