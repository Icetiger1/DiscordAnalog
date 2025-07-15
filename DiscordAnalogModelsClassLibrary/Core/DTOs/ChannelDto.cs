using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordAnalogModelsClassLibrary.Core.DTOs
{
    public record ChannelDto(
        string Id,
        string Name,
        bool IsVoiceChannel,
        List<MessageDto> Messages);
}
