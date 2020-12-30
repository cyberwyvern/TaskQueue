using System.ComponentModel.DataAnnotations;

namespace TaskQueue.Authentication
{
    public class AuthModel
    {
        [Required]
        [StringLength(64)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}