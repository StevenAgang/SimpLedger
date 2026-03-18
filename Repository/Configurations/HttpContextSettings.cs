namespace SimpLedger.Repository.Configurations
{
    public class HttpContextSettings
    {
        public bool InDevelopment { get; set; }
        public bool IsHttpOnly { get; set; }
        public bool IsSecure { get; set; }
        public string? SameSite { get; set; }
        public int ExpireInMinutes { get; set; }
    }
}
