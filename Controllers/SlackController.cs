using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SlackAPI;
using SlackeverBot.Models;
using SlackeverBot.Services;
using System.Threading.Tasks;

namespace SlakeverBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlackController : ControllerBase
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
            //var loggedString = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            //string requestString = await ReadRequestBody(Request.Body);
            //Console.WriteLine(loggedString);

            if (message.Type == EventType.UrlVerification)
            {
                return message.Challenge;
            }

            if (message.Type == EventType.EventCallback && message.Event.Type == MessageType.Message)
            {
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

        //async Task<string> ReadRequestBody(Stream requestBody)
        //{
        //    using (StreamReader reader = new StreamReader(requestBody))
        //    {
        //        //requestBody.Seek(0, SeekOrigin.Begin);
        //        return await reader.ReadToEndAsync();
        //    }
        //}
    }
}
