using ChatApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task<IActionResult> Index()
        {
            var users=await _messageService.GetUsers();
            return View(users);
        }
        public async Task<IActionResult>Chat(string selecteedUserId)
        {
            var chatViewModel=await _messageService.GetMessages(selecteedUserId);
            return View(chatViewModel);
        }
    }
}
