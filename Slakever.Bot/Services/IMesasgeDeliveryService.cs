using System;
using System.Threading.Tasks;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public interface IMessageDeliveryService
    {
        Task<string> Deliver(DeliveredMessageSet content, DeliveryType deliveryType = DeliveryType.Raw);
    }
}
