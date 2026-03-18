namespace SimpLedger.Repository.ViewModels.Emailing
{
    public class EmailSenderViewModel
    {
        public required string ToName { get; set; }
        public required string ToMail { get; set; }
        public required string Subject { get; set; }
        public required string Message { get; set; }
        public required string HeaderMessage { get; set; }

    }
}
