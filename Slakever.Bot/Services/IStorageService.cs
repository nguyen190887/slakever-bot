using System;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface IStorageService
    {
        Task Add(StoredMessage message);

        Task SaveToFile(string filePath, string content);
    }
}
