using System;
using System.Collections.Generic;

namespace SlakeverBot.Models
{
    public class Channel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedTime { get; set; }

        public IList<User> Members { get; set; }

        public IList<SlackMessage> PinnedMessages { get; set; }
    }

}
