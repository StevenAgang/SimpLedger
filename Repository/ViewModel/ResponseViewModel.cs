using System.Text.Json.Serialization;

namespace SimpLedger.Repository.ViewModel
{
    public class ResponseViewModel
    {
        public int status { get; set; }
        public bool success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? content { get; set; }
    }
}
