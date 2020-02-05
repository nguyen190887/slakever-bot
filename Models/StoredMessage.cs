using System;
namespace SlackeverBot.Models
{
    public class StoredMessage
    {
        public string Id { get; set; }
        public string Channel { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
