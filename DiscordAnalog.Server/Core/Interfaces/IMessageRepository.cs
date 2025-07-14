using DiscordAnalog.Server.API.Extensions;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary> Репозиторий для работы с сообщениями </summary>
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(int id);
        Task<IEnumerable<Message>> GetByRoomAsync(int roomId, int count);
        Task AddAsync(Message message);
    }
}
