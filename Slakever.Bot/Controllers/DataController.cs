using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SlakeverBot.Constants;
using SlakeverBot.Models;
using SlakeverBot.Services;

namespace SlakeverBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageQueryService _msgQueryService;
        private readonly IMessageDeliveryService _msgDeliveryService;

        public DataController(IMapper mapper, IMessageQueryService msgQueryService, IMessageDeliveryService msgDeliveryService)
        {
            _mapper = mapper;
            _msgQueryService = msgQueryService;
            _msgDeliveryService = msgDeliveryService;
        }

        [Route("stats")]
        public async Task<IEnumerable<ChannelStatInfo>> Stats(string date = null)
        {
            return await _msgQueryService.GetChannelMessageStats(ParseDateParam(date));
        }

        [Route("archive")]
        [HttpPost]
        public async Task<string> Archive(string date)
        {
            var archivedDate = ParseDateParam(date);
            var content = await _msgQueryService.LoadArchivedMessages(archivedDate);
            return await _msgDeliveryService.Deliver(content);
        }

        private DateTime ParseDateParam(string date)
        {
            DateTime.TryParseExact(date, AppConstants.FileDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);
            return parsedDate;
        }
    }
}
