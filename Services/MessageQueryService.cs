﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class MessageQueryService : IMessageQueryService
    {
        private readonly ISlackService _slackService;

        public MessageQueryService(ISlackService slackService)
        {
            _slackService = slackService;
        }

        public async Task<IEnumerable<ChannelStatInfo>> GetChannelMessageStats(DateTime date)
        {
            var stats = new List<ChannelStatInfo>();

            var channels = await _slackService.GetAllChannels();

            var fileTasks = new List<Task<Tuple<long, DateTime>>>();

            foreach (var channel in channels)
            {
                stats.Add(new ChannelStatInfo { Channel = channel });

                fileTasks.Add(Task.Run<Tuple<long, DateTime>>(() =>
                {
                    var filePath = Path.Combine("messages", $"{channel.Id}_{date.ToString("yyyyMMdd")}.txt");
                    if (File.Exists(filePath))
                    {
                        var fileInfo = new FileInfo(filePath);
                        return Tuple.Create(fileInfo.Length, fileInfo.LastWriteTimeUtc);
                    }
                    return Tuple.Create(0L, DateTime.MinValue);
                }));
            }

            var taskResults = await Task.WhenAll(fileTasks);
            for (var index = 0; index < taskResults.Length; index++)
            {
                stats[index].MessageSize = taskResults[index].Item1;
                stats[index].LastUpdated = taskResults[index].Item2;
            }

            return stats;
        }
    }
}