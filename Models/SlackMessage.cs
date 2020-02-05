using System;
using Newtonsoft.Json;

namespace SlackeverBot.Models
{
    public static class MessageType
    {
        public const string Message = "message";
        public const string PinAdded = "pin_added";
    }

    public static class EventType
    {
        public const string UrlVerification = "url_verification";
        public const string EventCallback = "event_callback";
    }

    public class SlackMessage
    {
        public string Token { get; set; }
        public string Challenge { get; set; }
        public string Type { get; set; }
        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        public MessageEvent Event { get; set; } = new MessageEvent();
        [JsonProperty("event_id")]
        public string EventId { get; set; }
        [JsonProperty("event_time")]
        public string EventTime { get; set; }
    }

    public class MessageEvent
    {
        [JsonProperty("client_msg_id")]
        public string MessageId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Team { get; set; }
        public string Channel { get; set; }
        [JsonProperty("channel_type")]
        public string ChannelType { get; set; }
    }
}
