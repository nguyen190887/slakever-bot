using System;
using System.Collections.Generic;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface IMessageQueryService
    {
        IEnumerable<ChannelStatInfo> GetChannelMessageStats(DateTime date);
    }
}
