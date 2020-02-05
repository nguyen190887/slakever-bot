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

    public class SlackMessage
    {
        public string Token { get; set; }
        public string Challenge { get; set; }
        public string Type { get; set; }
        //[JsonProperty("team_id")]
        public string Team_id { get; set; }
        //[JsonPropertyName("team_id")]
        public string TeamId => Team_id; // TODO: workaround

        public MessageEvent Event { get; set; } = new MessageEvent();
        //[JsonProperty("event_id")]
        public string Event_id { get; set; }
        //[JsonPropertyName("event_id")]
        public string EventId => Event_id; // TODO: workaround
        //[JsonProperty("event_time")]
        public string Event_time { get; set; }
        //[JsonPropertyName("event_time")]
        public string EventTime => Event_time; // TODO: workaround
    }

    public class MessageEvent
    {
        //[JsonProperty("client_msg_id")]
        public string Client_msg_id { get; set; }
        //[JsonPropertyName("client_msg_id")]
        public string MessageId => Client_msg_id; // TODO: workaround
        public string Type { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Team { get; set; }
        public string Channel { get; set; }
        public string Channel_type { get; set; }
        //[JsonPropertyName("channel_type")]
        public string ChannelType => Channel_type; // TODO: workaround
        public string Event_ts { get; set; }
        //[JsonPropertyName("event_ts")]
        public string EventTimestamp => Event_ts; // TODO: workaround
        public string Thread_ts { get; set; }
        //[JsonPropertyName("thread_ts")]
        public string ThreadTimestamp => Thread_ts; // TODO: workaround
    }
}
