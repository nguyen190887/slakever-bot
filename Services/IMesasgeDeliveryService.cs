using System;
using System.Threading.Tasks;

namespace SlakeverBot.Services
{
    public interface IMessageDeliveryService
    {
        Task Deliver(object content);
    }
}
