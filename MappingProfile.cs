using System;
using AutoMapper;
using SlakeverBot.Models;

namespace SlakeverBot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SlackMessage, StoredMessage>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Event.MessageId))
                .ForMember(d => d.Channel, m => m.MapFrom(s => s.Event.Channel))
                .ForMember(d => d.From, m => m.MapFrom(s => s.Event.User))
                .ForMember(d => d.Text, m => m.MapFrom(s => s.Event.Text))
                .ForMember(d => d.Timestamp, m => m.MapFrom(s => new DateTime(s.EventTime)))
                .ForMember(d => d.EventTimestamp, m => m.MapFrom(s => s.Event.EventTimestamp))
                .ForMember(d => d.ThreadTimestamp, m => m.MapFrom(s => s.Event.ThreadTimestamp));

            CreateMap<SlackAPI.Channel, Channel>()
                .ForMember(d => d.Description, m => m.MapFrom(s => s.purpose))
                .ForMember(d => d.MemberIds, m => m.MapFrom(s => s.members))
                .ForMember(d => d.Members, m => m.Ignore());

            CreateMap<SlackAPI.User, User>();
        }
    }
}
