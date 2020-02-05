using System;
using System.IO;
using System.Threading.Tasks;
using SlackeverBot.Models;

namespace SlackeverBot.Services
{
    public class FileStorageService: IStorageService
    {
        private const string MessageFolder = "messages";

        static FileStorageService()
        {
            if (!Directory.Exists(MessageFolder))
            {
                Directory.CreateDirectory(MessageFolder);
            }
        }

        public async Task Add(StoredMessage message)
        {
            // TODO: update message
            await File.AppendAllTextAsync(GetFileName(message.Channel), $"{message.Text}{Environment.NewLine}");
        }

        private string GetFileName(string channelId)
        {
            return Path.Combine(MessageFolder, $"{channelId}_{DateTime.UtcNow.ToString("yyyyMMdd")}.txt");
        }
    }
}
