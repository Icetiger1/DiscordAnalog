using AutoMapper;
using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Core.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiscordAnalog.Server.Infrastructure.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp.ToString("g")));
            
            // Пример дополнительных маппингов:
            // CreateMap<User, UserDto>();
            // CreateMap<ChatRoom, ChatRoomDto>();
        }
    }

}
