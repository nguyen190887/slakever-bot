using System;
using AutoMapper;
using SlackeverBot.Models;

namespace SlakeverBot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SlackMessage, StoredMessage>()
                .ForMember(s => s.Id, m => m.MapFrom(s => s.Event.MessageId))
                .ForMember(s => s.Channel, m => m.MapFrom(s => s.Event.Channel))
                .ForMember(s => s.From, m => m.MapFrom(s => s.Event.User))
                .ForMember(s => s.Text, m => m.MapFrom(s => s.Event.Text))
                .ForMember(s => s.Timestamp, m => m.MapFrom(s => new DateTime(s.EventTime)))
                .ForMember(s => s.EventTimestamp, m => m.MapFrom(s => s.Event.EventTimestamp))
                .ForMember(s => s.ThreadTimestamp, m => m.MapFrom(s => s.Event.ThreadTimestamp));
        }
    }
}
