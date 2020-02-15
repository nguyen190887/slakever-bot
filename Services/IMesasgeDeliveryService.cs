using System;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface IMessageDeliveryService
    {
        Task Deliver(DeliveredMessageSet content);
    }
}
