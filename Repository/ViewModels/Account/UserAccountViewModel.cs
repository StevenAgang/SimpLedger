namespace SimpLedger.Repository.ViewModels.Account
{
    public class UserAccountViewModel
    {
    }

    public class UserAccountLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class AuthenticationResponse
    {
        public required int Id { get; set; }
        public required string Token { get; set; }
        public required string FullName { get; set; }

    }
}
