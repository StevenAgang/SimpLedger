namespace SimpLedger.Repository.ViewModels.Verification
{
    public class VerificationViewModel
    {
    }

    public class EmailVerificationRequestField
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
