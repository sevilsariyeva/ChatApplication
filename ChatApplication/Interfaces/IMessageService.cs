using ChatApplication.ViewModels.MessagesViewModels;

namespace ChatApplication.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageUsersListViewModel>> GetUsers();
        Task<ChatViewModel> GetMessages(string selectedUserId);
    }
}
