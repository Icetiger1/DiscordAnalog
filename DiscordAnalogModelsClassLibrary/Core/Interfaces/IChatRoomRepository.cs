using DiscordAnalogModelsClassLibrary.Core.Entities;

namespace DiscordAnalogModelsClassLibrary.Core.Interfaces;

// Core/Interfaces/IChatRoomRepository.cs
public interface IChatRoomRepository
{
    Task<ChatRoom?> GetByIdAsync(int roomId);
    Task<IEnumerable<Message>> GetMessagesAsync(int roomId, int count);
    Task AddMessageAsync(int roomId, Message message);
}

