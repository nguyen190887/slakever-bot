﻿using System;
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
                    RenderChatPageContent(channelData.ChannelName, channelData.ChatDate, BuildHtmlChat(fileName, channelData)));
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
            TagBuilder container = new TagBuilder("div");
            container.InnerHtml.AppendHtml(GenerateGlobalStyles());

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

                    chatLine.InnerHtml.AppendHtml("<div class='arrow-up'></div>");
                    chatLine.InnerHtml.AppendHtml(childMessageContainer);
                }

                container.InnerHtml.AppendHtml(chatLine);
            }

            var stringWriter = new StringWriter();
            container.WriteTo(stringWriter, System.Text.Encodings.Web.HtmlEncoder.Default);

            return stringWriter.ToString();
        }

        private TagBuilder BuildHtmlChatLine(DeliveredMessage msg)
        {
            var chatLine = new TagBuilder("li");
            chatLine.InnerHtml.AppendHtml(
                $"<i>{msg.Timestamp}</i>&nbsp;|&nbsp;<i class='user'>{msg.UserName}</i>: <span>{msg.Text}</span>");
            return chatLine;
        }

        private string GenerateGlobalStyles()
        {
            return @"
<style>
ul, li {
  list-style: none;
  margin: 5px;
}

.user {
  font-weight: bolder;
}

.child-messages {
  background: #eee;
  padding-left: 0;
  margin-left: 10px;
  margin-top: 0;
}

.child-messages li {
  margin-top: 0;
}

.arrow-up {
  width: 0;
  height: 0;
  border-left: 5px solid transparent;
  border-right: 5px solid transparent;
  border-bottom: 8px solid #eee;
  margin-left: 15px;
}
</style>
";
        }

        private string RenderChatPageContent(string channelName, DateTime chatDate, string htmlChat)
        {
            const string pageTemplate =
                @"<!DOCTYPE html>
                <html lang=""en"">
                <head>
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                  <meta charset=""utf-8"">
                  <meta name=""viewport"" content=""width=device-width"">
                  <title>Channel: {0} | {1}</title>
                </head>
                <body>
                {2}
                </body>
                </html>
                ";

            return string.Format(pageTemplate, channelName, chatDate.ToShortDateString(), htmlChat);
        }
    }
}
