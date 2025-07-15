using AutoMapper;
using DiscordAnalogModelsClassLibrary.Core.DTOs;
using DiscordAnalogModelsClassLibrary.Core.Entities;

namespace DiscordAnalog.Server.Infrastructure.Data
{
    /// <summary>
    /// MappingProfile
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
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
