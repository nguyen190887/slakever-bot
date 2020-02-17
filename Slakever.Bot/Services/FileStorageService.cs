using System;
using System.IO;
using System.Threading.Tasks;
using SlakeverBot.Constants;
using SlakeverBot.Models;
using SlakeverBot.Utils;

namespace SlakeverBot.Services
{
    public class FileStorageService : IStorageService
    {
        //private static readonly string MessageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "messages");

        static FileStorageService()
        {
            Console.WriteLine("[INFO] Message folder: " + FileConstants.MessageFolder);
            if (!Directory.Exists(FileConstants.MessageFolder))
            {
                Directory.CreateDirectory(FileConstants.MessageFolder);
            }
        }

        public async Task Add(StoredMessage message)
        {
            await File.AppendAllTextAsync(GetFileName(message.Channel), $"{message.ToLogString()}{Environment.NewLine}");
        }

        private string GetFileName(string channelId)
        {
            return Path.Combine(FileConstants.MessageFolder, $"{channelId}_{DateTime.UtcNow.ToFileString()}.txt");
        }
    }
}
