using System;
using System.Collections.Generic;

namespace SlakeverBot.Models
{
    public abstract class DeliveredMessage
    {
        public DateTime Timestamp { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public DeliveredMessage() { }

        public DeliveredMessage(DeliveredMessage copy)
        {
            Timestamp = copy.Timestamp;
            UserName = copy.UserName;
            Text = copy.Text;
        }
    }

    public class ChannelDeliveredMessage: DeliveredMessage
    {
        public IList<DeliveredMessage> ChildMessages { get; set; }

        public ChannelDeliveredMessage() { }

        public ChannelDeliveredMessage(DeliveredMessage copy) : base(copy) { }
    }

    public class ThreadDeliveredMessage: DeliveredMessage
    {
        public DeliveredMessage ParentMessage { get; set; }

        public ThreadDeliveredMessage() { }

        public ThreadDeliveredMessage(DeliveredMessage copy) : base(copy) { }
    }

    public class DeliveredMessageCollection : List<DeliveredMessage>
    {

    }
}
