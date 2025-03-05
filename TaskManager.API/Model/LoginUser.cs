using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Model
{
    public class LoginUser
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
