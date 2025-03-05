using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Model
{
    public class RegisterUser
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
    }
}
