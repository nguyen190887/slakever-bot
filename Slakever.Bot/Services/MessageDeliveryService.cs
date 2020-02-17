using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class MessageDeliveryService : IMessageDeliveryService
    {
        public Task<string> Deliver(DeliveredMessageSet msgSet)
        {
            var sb = new StringBuilder();
            foreach (string fileName in msgSet.Keys)
            {
                var channelData = msgSet[fileName];
                var log = BuildLog(fileName, channelData);

                Console.WriteLine("** File: " + fileName);
                Console.WriteLine(log);

                sb.AppendLine($"######{Environment.NewLine}Channel: {channelData.ChannelName} - File: {fileName}{Environment.NewLine}{log}");
                sb.AppendLine(Environment.NewLine);
            }

            return Task.FromResult(sb.ToString());
        }

        private string BuildLog(string fileName, ChannelMessageSet messages)
        {
            var sb = new StringBuilder();
            var nestedIndent = new string(' ', 4);

            foreach (var msg in messages)
            {
                sb.AppendLine(msg.ToString());

                foreach (var childMsg in ((ChannelDeliveredMessage)msg).ChildMessages)
                {
                    sb.AppendLine($"{nestedIndent}{childMsg.ToString()}");
                }
            }

            return sb.ToString();
        }
    }
}
