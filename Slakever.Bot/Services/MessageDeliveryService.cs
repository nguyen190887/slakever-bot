using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlakeverBot.Models;

namespace SlakeverBot.Services
{
    public enum DeliveryType
    {
        Raw,
        Html
    }

    public class MessageDeliveryService : IMessageDeliveryService
    {
        private readonly IFileSharingService _sharingService;
        private readonly IStorageService _storageService;
        private readonly ISlackService _slackService;

        public MessageDeliveryService(IFileSharingService sharingService, IStorageService storageService, ISlackService slackService)
        {
            _sharingService = sharingService;
            _storageService = storageService;
            _slackService = slackService;
        }

        public async Task<string> Deliver(DeliveredMessageSet msgSet, DeliveryType deliveryType = DeliveryType.Raw)
        {
            string messages = string.Empty;

            switch (deliveryType)
            {
                case DeliveryType.Html:
                    await BuildAndDeliverHtmlMessages(msgSet);
                    break;
                default:
                    messages = BuildRawMessages(msgSet);
                    break;
            }

            return messages;
        }

        private string BuildRawMessages(DeliveredMessageSet msgSet)
        {
            var sb = new StringBuilder();
            foreach (string fileName in msgSet.Keys)
            {
                var channelData = msgSet[fileName];
                var log = BuildLog(fileName, channelData);

                Console.WriteLine("** File: " + fileName);
                Console.WriteLine(log);

                sb.AppendLine($"######{Environment.NewLine}Channel: {channelData.ChannelName} - File: {fileName}{Environment.NewLine}{log}");
                sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString();
        }

        private async Task BuildAndDeliverHtmlMessages(DeliveredMessageSet msgSet)
        {
            foreach (string fileName in msgSet.Keys)
            {
                var channelData = msgSet[fileName];

                // for testing
                await _storageService.SaveToFile(
                    Path.Combine("saved", $"{fileName}.html"),
                    RenderChatPageContent(channelData.ChannelName, BuildHtmlChat(fileName, channelData)));
            }
        }

        private string BuildLog(string fileName, ChannelMessageSet messages)
        {
            var sb = new StringBuilder();
            var nestedIndent = new string(' ', 4);

            foreach (var msg in messages)
            {
                sb.AppendLine(msg.ToString());

                foreach (var childMsg in ((ChannelDeliveredMessage)msg).ChildMessages)
                {
                    sb.AppendLine($"{nestedIndent}{childMsg.ToString()}");
                }
            }

            return sb.ToString();
        }

        private string BuildHtmlChat(string fileName, ChannelMessageSet messages)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            foreach (var msg in messages)
            {
                var chatLine = BuildHtmlChatLine(msg);

                var childMessages = ((ChannelDeliveredMessage)msg).ChildMessages;
                if (childMessages.Any())
                {
                    var childMessageContainer = new TagBuilder("ul");
                    childMessageContainer.AddCssClass("child-messages");

                    foreach (var childMsg in childMessages)
                    {
                        childMessageContainer.InnerHtml.AppendHtml(BuildHtmlChatLine(childMsg));
                    }

                    chatLine.InnerHtml.AppendHtml(childMessageContainer);
                }

                tagBuilder.InnerHtml.AppendHtml(chatLine);
            }

            var stringWriter = new StringWriter();
            tagBuilder.WriteTo(stringWriter, System.Text.Encodings.Web.HtmlEncoder.Default);

            return stringWriter.ToString();
        }

        private string RenderChatPageContent(string channelName, string htmlChat)
        {
            const string pageTemplate =
                @"<!DOCTYPE html>
                <html lang=""en"">
                <head>
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                  <meta charset=""utf-8"">
                  <meta name=""viewport"" content=""width=device-width"">
                  <title>{0}</title>
                </head>
                <body>
                {1}
                </body>
                </html>
                ";

            return string.Format(pageTemplate, channelName, htmlChat);
        }

        private TagBuilder BuildHtmlChatLine(DeliveredMessage msg)
        {
            var chatLine = new TagBuilder("li");
            chatLine.InnerHtml.AppendHtml(
                $"<i>{msg.Timestamp}</i> | <i>{msg.UserName}</i>: <span>{msg.Text}</span>");
            return chatLine;
        }
    }
}
