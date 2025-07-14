using DiscordAnalog.Server.API.Extensions;

namespace DiscordAnalog.Server.Core.Interfaces
{
    /// <summary> Сервис для работы с JWT-токенами </summary>
    public interface ITokenService
    {
        string GenerateToken(User user);
        // Можно добавить методы валидации, обновления токена и т.д.
    }
}
