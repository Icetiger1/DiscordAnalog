using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DiscordAnalog.Server.API.Controllers
{
    /// <summary>
    /// Контроллер авторизации
    /// </summary>
    [Route("api/[controller]")] // => /api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Конструктор контроллера авторизации
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tokenService"></param>
        public AuthController(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (await _userRepository.GetByUsernameAsync(request.Username) != null)
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Id = Guid.NewGuid().ToString()
            };

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Username));
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Username));
        }
    }

    /// <summary>
    /// Модель запроса авторизации
    /// </summary>
    /// <param name="Username">имя пользователя</param>
    /// <param name="Password">пароль пользователя</param>
    public record AuthRequest(string Username, string Password);


    /// <summary>
    /// Модель ответа на запрос авторизации
    /// </summary>
    /// <param name="Token">токен авторизации пользователя</param>
    /// <param name="Username">имя пользователя</param>
    public record AuthResponse(string Token, string Username);
}
