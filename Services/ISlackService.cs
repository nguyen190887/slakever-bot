using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface ISlackService
    {
        Task<IEnumerable<Channel>> GetAllChannels();

        Task<Channel> GetChannelInfo(string channelId);

        Task<User> GetUserInfo(string userId);
    }
}
