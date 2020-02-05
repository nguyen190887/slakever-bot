using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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

    [DataContract]
    public class SlackMessage
    {
        public string Token { get; set; }
        public string Challenge { get; set; }
        public string Type { get; set; }
        //[JsonProperty("team_id")]
        [JsonPropertyName("team_id")]
        public string TeamId { get; set; }

        public MessageEvent Event { get; set; }// = new MessageEvent();
        //[JsonProperty("event_id")]
        [JsonPropertyName("event_id")]
        public string EventId { get; set; }
        //[JsonProperty("event_time")]
        [JsonPropertyName("event_time")]
        public string EventTime { get; set; }
    }

    [DataContract]
    public class MessageEvent
    {
        //[JsonProperty("client_msg_id")]
        [JsonPropertyName("client_msg_id")]
        public string MessageId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Team { get; set; }
        public string Channel { get; set; }
        [JsonPropertyName("channel_type")]
        public string ChannelType { get; set; }
        [JsonPropertyName("event_ts")]
        public string EventTimestamp { get; set; }
        [JsonPropertyName("thread_ts")]
        public string ThreadTimestamp { get; set; }
    }
}
