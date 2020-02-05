using System;
using System.Threading.Tasks;
using SlackeverBot.Models;

namespace SlackeverBot.Services
{
    public interface IStorageService
    {
        Task Add(StoredMessage message);
    }    
}
