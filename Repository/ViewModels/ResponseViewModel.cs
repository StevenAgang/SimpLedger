using System.Text.Json.Serialization;

namespace SimpLedger.Repository.ViewModel
{
    public class ResponseViewModel
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Content { get; set; }
    }
}
