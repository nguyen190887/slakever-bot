using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using SlakeverBot.Models;
using SlakeverBot.Constants;

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

            var channels = await _slackService.GetAllChannels();  // TODO: exclude channels which do not contain bot

            var fileTasks = new List<Task<Tuple<long, DateTime>>>();

            foreach (var channel in channels)
            {
                stats.Add(new ChannelStatInfo { Channel = channel });

                AllocateFileTasks(fileTasks, date, channel);
            }

            var taskResults = await Task.WhenAll(fileTasks);
            for (var index = 0; index < taskResults.Length; index++)
            {
                stats[index].MessageSize = taskResults[index].Item1;
                stats[index].LastUpdated = taskResults[index].Item2;
            }

            // Fetch user info
            foreach (var s in stats)
            {
                s.Channel.Members = s.Channel.MemberIds
                    .Select(id => _slackService.GetUserInfo(id).Result)
                    .ToList();
            }

            return stats;
        }

        private static void AllocateFileTasks(List<Task<Tuple<long, DateTime>>> fileTasks, DateTime date, Channel channel)
        {
            Console.WriteLine($"Reading file from {FileConstants.MessageFolder} folder.");
            System.Diagnostics.Debug.WriteLine($"Reading file from {FileConstants.MessageFolder} folder."); // TODO: just use 1 log

            fileTasks.Add(Task.Run<Tuple<long, DateTime>>(() =>
            {
                var filePath = Path.Combine(FileConstants.MessageFolder, $"{channel.Id}_{date.ToString("yyyyMMdd")}.txt");
                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    return Tuple.Create(fileInfo.Length, fileInfo.LastWriteTimeUtc);
                }
                return Tuple.Create(0L, DateTime.MinValue);
            }));
        }

        public Task<object> LoadArchivedMessages(DateTime archivedDate)
        {
            throw new NotImplementedException();
        }
    }
}
