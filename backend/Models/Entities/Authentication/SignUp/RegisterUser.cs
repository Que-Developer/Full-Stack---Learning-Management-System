namespace backend.Models.Entities.Authentication.SignUp
{
    public class RegisterUser
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
