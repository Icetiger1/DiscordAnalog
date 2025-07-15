using DiscordAnalogModelsClassLibrary.Core.Entities;

namespace DiscordAnalogModelsClassLibrary.Core.Interfaces;
/// <summary> Репозиторий для работы с пользователями </summary>
public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}

