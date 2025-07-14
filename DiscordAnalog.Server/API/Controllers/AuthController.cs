using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Core.Interfaces;
using DiscordAnalog.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DiscordAnalog.Server.API.Controllers
{
    [Route("api/[controller]")] // => /api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        /// <summary> Регистрация нового пользователя </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (await _userRepository.GetByUsernameAsync(request.Username) != null)
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Username));
        }

        /// <summary> Вход в систему </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Username));
        }
    }

    // DTO для запросов
    public record AuthRequest(string Username, string Password);
    public record AuthResponse(string Token, string Username);
}
