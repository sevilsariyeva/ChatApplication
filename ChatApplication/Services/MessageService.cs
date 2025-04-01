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
            var messages = await _context.Messages.Where(i => (i.SenderId == currentUserId || i.SenderId == selectedUserId) && (i.ReceiverId == currentUserId)).Select(i => new UserMessagesListViewModel()
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

        public Task<IEnumerable<MessageUsersListViewModel>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
