using DiscordAnalog.Server.API.Extensions;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary> Репозиторий для работы с пользователями </summary>
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
