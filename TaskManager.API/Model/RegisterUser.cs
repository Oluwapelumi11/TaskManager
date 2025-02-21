namespace TaskManager.API.Model
{
    public class RegisterUser
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
