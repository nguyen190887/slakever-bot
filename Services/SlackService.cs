using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class SlackService : ISlackService
    {
        // TODO: move to cache later
        private static readonly Dictionary<string, SlackAPI.Channel> _cacheChannels = new Dictionary<string, SlackAPI.Channel>();
        private static readonly object _channelCacheLock = new object();

        private readonly SlackAPI.SlackTaskClient _slackClient;

        private readonly IMapper _mapper;

        public SlackService(IConfiguration configuration, IMapper mapper)
        {
            _slackClient = new SlackAPI.SlackTaskClient(configuration.GetValue<string>("SLACK_TOKEN"));
            _mapper = mapper;
        }

        public async Task<Channel> GetChannelInfo(string channelId)
        {
            if (_cacheChannels.TryGetValue(channelId, out SlackAPI.Channel channel))
            {
                return _mapper.Map<Channel>(channel);
            }

            var channels = await _slackClient.GetChannelListAsync();
            //const string TOKEN = "TODO-TBD";
            //var slackClient = new SlackTaskClient(TOKEN);

            //var response = await slackClient.PostMessageAsync("#general", msg);
            throw new NotImplementedException();
        }

        public User GetUserInfo(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
