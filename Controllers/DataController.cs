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
        public async Task<IEnumerable<ChannelStatInfo>> Stats()
        {
            var testDate = new DateTime(2020, 2, 5);
            return await _messageQueryService.GetChannelMessageStats(testDate);
        }
    }
}
