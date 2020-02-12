using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SlackAPI;
using SlakeverBot.Models;
using SlakeverBot.Services;

namespace SlakeverBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageQueryService _messageQueryService;

        public DataController(IMapper mapper, IMessageQueryService messageQueryService)
        {
            _mapper = mapper;
            _messageQueryService = messageQueryService;
        }

        [Route("stats")]
        public async Task<IEnumerable<ChannelStatInfo>> Stats(string date = null)
        {
            DateTime.TryParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate);
            return await _messageQueryService.GetChannelMessageStats(parsedDate);
        }
    }
}
