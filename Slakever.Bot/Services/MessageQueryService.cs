﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using SlakeverBot.Models;
using SlakeverBot.Constants;
using SlakeverBot.Utils;
using System.Text.RegularExpressions;

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
                var filePath = Path.Combine(FileConstants.MessageFolder, $"{channel.Id}_{date.ToFileString()}.txt");
                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    return Tuple.Create(fileInfo.Length, fileInfo.LastWriteTimeUtc);
                }
                return Tuple.Create(0L, DateTime.MinValue);
            }));
        }

        public async Task<DeliveredMessageSet> LoadArchivedMessages(DateTime archivedDate)
        {
            var searchPattern = $"*_{archivedDate.ToFileString()}.txt";
            var filePaths = Directory.GetFiles(FileConstants.MessageFolder, searchPattern);

            var deliveredMessageSet = new DeliveredMessageSet();

            foreach (var path in filePaths)
            {
                Dictionary<string, DeliveredMessage> parsedChannelMessageDict = new Dictionary<string, DeliveredMessage>();

                using (StreamReader reader = new StreamReader(File.OpenRead(path)))
                {
                    const string msgTimestampPattern = "[0-9]+\\.[0-9]{6}";
                    var msgTsRegex = new Regex(msgTimestampPattern);

                    string line;
                    DeliveredMessage currentMsg = null;


                    while ((line = reader.ReadLine()) != null)
                    {
                        var wordGroup = line.Split('\t');

                        if (wordGroup.Length == 3 && msgTsRegex.IsMatch(wordGroup[0]))
                        {
                            var rawTimestamp = wordGroup[0];
                            currentMsg = await MapToMessage<ChannelDeliveredMessage>(wordGroup);
                            parsedChannelMessageDict[rawTimestamp] = currentMsg;
                        }
                        // thread msg
                        else if (msgTsRegex.IsMatch(wordGroup[wordGroup.Length - 1]))
                        {
                            if (wordGroup.Length == 4)
                            {
                                currentMsg = await MapToMessage<ThreadDeliveredMessage>(wordGroup);

                                if (parsedChannelMessageDict.TryGetValue(wordGroup[3], out DeliveredMessage parentMsg))
                                {
                                    ((ThreadDeliveredMessage)currentMsg).ParentMessage = parentMsg;
                                    ((ChannelDeliveredMessage)parentMsg).ChildMessages.Add(currentMsg);
                                }
                            }
                            else if (wordGroup.Length == 2)
                            {
                                if (currentMsg != null)
                                {
                                    currentMsg = new ThreadDeliveredMessage(currentMsg);
                                    currentMsg.Text += $"{Environment.NewLine}{wordGroup[0]}";

                                    if (parsedChannelMessageDict.TryGetValue(wordGroup[1], out DeliveredMessage parentMsg))
                                    {
                                        ((ThreadDeliveredMessage)currentMsg).ParentMessage = parentMsg;
                                        ((ChannelDeliveredMessage)parentMsg).ChildMessages.Add(currentMsg);
                                    }
                                }
                            }
                        }
                        // text spans multi lines
                        else
                        {
                            if (currentMsg != null)
                            {
                                currentMsg.Text += $"{Environment.NewLine}{wordGroup[0]}";
                            }
                        }
                    }
                }

                string logFile = Path.GetFileName(path);
                deliveredMessageSet[logFile] = new ChannelMessageSet(parsedChannelMessageDict.Values)
                {
                    ChannelName = await GetChannelName(logFile)
                };

            }

            return deliveredMessageSet;
        }

        private async Task<string> GetChannelName(string logFile)
        {
            var channelId = logFile.Split('_')[0];
            return (await _slackService.GetChannelInfo(channelId))?.Name ?? channelId;
        }

        private DateTime ExtractTimeStamp(string ts)
        {
            try
            {
                return (long.Parse(ts.Split('.')[0])).FromUnixTime();
            }
            catch { }
            return DateTime.MinValue;
        }

        private async Task<T> MapToMessage<T>(string[] wordGroup) where T : DeliveredMessage, new()
        {
            return new T
            {
                Timestamp = ExtractTimeStamp(wordGroup[0]),
                UserName = (await _slackService.GetUserInfo(wordGroup[1]))?.Name ?? wordGroup[1], // TODO: consider process separately
                Text = wordGroup[2]
            };
        }
    }
}