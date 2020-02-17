using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public class MessageDeliveryService : IMessageDeliveryService
    {
        public Task Deliver(DeliveredMessageSet msgSet)
        {
            foreach (string fileName in msgSet.Keys)
            {
                LogResult(fileName, msgSet[fileName]);
            }

            return Task.FromResult("");
        }

        private void LogResult(string fileName, IList<DeliveredMessage> messages)
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

            Console.WriteLine("** File: " + fileName);
            Console.WriteLine(sb);
        }
    }
}
