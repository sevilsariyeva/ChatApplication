using ChatApplication.Data;
using ChatApplication.Helpers;
using ChatApplication.Interfaces;
using ChatApplication.ViewModels.MessagesViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private ICurrentUserService _currentUserService;
        public MessageService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<ChatViewModel> GetMessages(string selectedUserId)
        {
            var currentUserId = _currentUserService.UserId;
            var selectedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == selectedUserId);
            var selectedUserName = "";
            if (selectedUser != null)
            {
                selectedUserName = selectedUser.UserName;
            }
            var chatViewModel = new ChatViewModel()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectedUserId,
                ReceiverUserName = selectedUserName
            };
            var messages = await _context.Messages.Where(i =>
                (i.SenderId == currentUserId && i.ReceiverId == selectedUserId) ||
                (i.SenderId == selectedUserId && i.ReceiverId == currentUserId))
            .Select(i => new UserMessagesListViewModel()
            { 
                Id = i.Id,
                Text = i.Text,
                Date = i.Date.ToShortDateString(),
                Time = i.Date.ToShortTimeString(),
                IsCurrentUserSentMessage = i.SenderId == currentUserId
            }).ToListAsync();

            chatViewModel.Messages = messages;
            return chatViewModel;

        }

        public async Task<IEnumerable<MessageUsersListViewModel>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;
            var users = await _context.Users
                .Where(i => i.Id != currentUserId)
                .Select(i => new MessageUsersListViewModel
                {
                    Id = i.Id,
                    Username = i.UserName,
                    LastMessage = _context.Messages.Where(m => (m.SenderId == currentUserId || m.SenderId == i.Id)&& (m.ReceiverId == currentUserId || m.ReceiverId == i.Id))
                        .OrderByDescending(m => m.Id)
                        .Select(m => m.Text)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return users;
        }
    }
}
