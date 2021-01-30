using System;
namespace SlakeverBot.Models
{
    public class StoredMessage
    {
        public string Id { get; set; }
        public string Channel { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventTimestamp { get; set; }
        public string ThreadTimestamp { get; set; }

        public string ToLogString() => $"{EventTimestamp}\t{From}\t{Text}" +
            (string.IsNullOrEmpty(ThreadTimestamp) ? "" : $"\t{ThreadTimestamp}");
    }
}
