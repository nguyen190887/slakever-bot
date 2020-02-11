using System;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface ISlackService
    {
        Task<Channel> GetChannelInfo(string channelId);

        User GetUserInfo(string userId);
    }
}
