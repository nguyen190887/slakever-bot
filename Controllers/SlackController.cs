using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlackAPI;
using SlackeverBot.Models;
using SlackeverBot.Services;

namespace SlakeverBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlackController: ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public SlackController(IMapper mapper, IStorageService storageService)
        {
            _mapper = mapper;
            _storageService = storageService;
        }

        [Route("home")]
        public string Index()
        {
            return "Welcome to Slakever!";
        }

        [HttpGet, HttpPost]
        [Route("events")]
        public async Task<string> Events([FromBody]SlackMessage message)
        {
            var loggedString = JsonConvert.SerializeObject(message);
            //string requestString = await ReadRequestBody(Request.Body);
            Console.WriteLine(loggedString);

            if (message.Type == EventType.UrlVerification)
            {
                return message.Challenge;
            }

            if (message.Type == EventType.EventCallback && message.Event.Type == MessageType.Message)
            {
                Console.WriteLine($"Received: {message.Event.Text}");
                await _storageService.Add(_mapper.Map<StoredMessage>(message));
            }

            //return new ContentResult { Content = requestString };
            return string.Empty;
        }

        [HttpGet]
        [Route("test")]
        public async Task Test(string msg)
        {
            const string TOKEN = "TODO-TBD";
            var slackClient = new SlackTaskClient(TOKEN);

            var response = await slackClient.PostMessageAsync("#general", msg);
        }

        async Task<string> ReadRequestBody(Stream requestBody)
        {
            using (StreamReader reader = new StreamReader(requestBody))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    /*
     private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
     */
}
