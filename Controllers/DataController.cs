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
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DataController(IMapper mapper, IStorageService storageService, IConfiguration configuration)
        {
            _mapper = mapper;
            _storageService = storageService;
            _configuration = configuration;
        }

        [Route("stats")]
        public string Stats()
        {
            return "Welcome to Slakever!";
        }
    }
}
