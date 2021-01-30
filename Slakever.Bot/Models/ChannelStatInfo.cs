using System;
namespace SlakeverBot.Models
{
    public class ChannelStatInfo
    {
        public Channel Channel { get; set; }

        public long MessageSize { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
