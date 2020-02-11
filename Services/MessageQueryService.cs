using System;
using System.Collections.Generic;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class MessageQueryService: IMessageQueryService
    {
        public MessageQueryService()
        {
        }

        public IEnumerable<ChannelStatInfo> GetChannelMessageStats(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
