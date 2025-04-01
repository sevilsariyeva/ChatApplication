using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Models
{
    public class AppUser:IdentityUser
    {
        [InverseProperty(nameof(Message.Sender))]
        public virtual ICollection<Message>SentMessages { get; set; }
        [InverseProperty(nameof(Message.Receiver))]
        public virtual ICollection<Message>ReceivedMessages { get; set; }
    }
}
