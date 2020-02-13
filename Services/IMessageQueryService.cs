using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface IMessageQueryService
    {
        Task<IEnumerable<ChannelStatInfo>> GetChannelMessageStats(DateTime date);
        Task<DeliveredMessageCollection> LoadArchivedMessages(DateTime archivedDate);
    }
}
