namespace SimpLedger.Repository.ViewModels.Account
{
    public class UserAccountViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Token { get; set; }

        // add more value when needed
    }

    public class UserAccountLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserAccountCreation : UserAccountLogin
    {
        public bool? Agreement { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required string UserType { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessAddress { get; set; }
        public string? BusinessEmail { get; set; }
    }

    public class ActivateAccountViewModel
    {
        public required string Token { get; set; }

    }

    public class  ActivateAccountResponse : ActivateAccountViewModel
    {
    }

    public class ActivateRequest
    {
        public string? Email { get; set; }
    }

    public class VerifyCode
    {
        public int Id { get; set; }
        public int Code { get; set; }
    }

    public class AuthenticationResponse
    {
        public required int Id { get; set; }
        public required string Token { get; set; }
        public required string FullName { get; set; }

    }
}
