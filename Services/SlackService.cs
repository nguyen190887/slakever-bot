using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class SlackService : ISlackService
    {
        // TODO: move to cache later
        private static readonly Dictionary<string, SlackAPI.Channel> _cachedChannels = new Dictionary<string, SlackAPI.Channel>();
        private static readonly object _channelCacheLock = new object();

        private static readonly Dictionary<string, SlackAPI.User> _cachedUsers = new Dictionary<string, SlackAPI.User>();
        private static readonly object _userCacheLock = new object();

        private readonly SlackAPI.SlackTaskClient _slackClient;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        public SlackService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _slackClient = new SlackAPI.SlackTaskClient(GetSlackToken());
            _mapper = mapper;
        }

        public async Task<IEnumerable<Channel>> GetAllChannels()
        {
            await EnsureChannelsFetched();

            var channels = new List<Channel>();
            foreach (var channel in _cachedChannels.Values)
            {
                channels.Add(_mapper.Map<Channel>(channel));
            }
            return channels;
        }

        public async Task<Channel> GetChannelInfo(string channelId)
        {
            await EnsureChannelsFetched();

            if (_cachedChannels.TryGetValue(channelId, out SlackAPI.Channel channel))
            {
                return _mapper.Map<Channel>(channel);
            }

            return null;
        }

        public async Task<User> GetUserInfo(string userId)
        {
            if (await EnsureUsersFetched())
            {

                if (_cachedUsers.TryGetValue(userId, out SlackAPI.User user))
                {
                    return _mapper.Map<User>(user);
                }
            }

            return null;
        }



        #region Utilities

        private string GetSlackToken()
        {
            var envToken = Environment.GetEnvironmentVariable("SLACK_TOKEN");
            var token = string.IsNullOrEmpty(envToken) ? _configuration.GetValue<string>("SlackToken") : envToken;

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Slack Token is missing.");
            }

            return token;
        }

        private async Task EnsureChannelsFetched()
        {
            if (!_cachedChannels.Any())
            {
                var channels = await _slackClient.GetChannelListAsync();

                lock (_channelCacheLock)
                {
                    foreach (var c in channels.channels)
                    {
                        _cachedChannels[c.id] = c;
                    }
                }
            }
        }

        private async Task<bool> EnsureUsersFetched()
        {
            if (!_cachedUsers.Any())
            {
                var users = await _slackClient.GetUserListAsync();
                if (users == null || users.members == null)
                {
                    return false;
                }

                lock (_userCacheLock)
                {
                    foreach (var u in users.members)
                    {
                        _cachedUsers[u.id] = u;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
