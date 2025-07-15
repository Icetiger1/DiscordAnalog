using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DiscordAnalog.Server.API.Controllers
{
    /// <summary>
    /// Контроллер пользователи
    /// </summary>
    [Route("api/[controller]")] // => /api/users
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Конструктор контроллера пользователей
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tokenService"></param>
        public UsersController(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Получения списка всех пользователей
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API</response>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _userRepository.GetAllUsersAsync();

            return Ok(users);
        }

        /// <summary>
        /// Поиск пользователя по имени
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API</response>
        [HttpGet("GetByName/{username}")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return NotFound("Invalid username");

            return Ok(new UserResponse(user.Id, user.Username, user.PasswordHash));
        }

        /// <summary>
        /// Поиск пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API</response>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("Invalid id");

            return Ok(new UserResponse(user.Id, user.Username, user.PasswordHash));
        }
    }


    /// <summary>
    /// Запрос пользователя по имени
    /// </summary>
    /// <param name="Username">имя пользователя</param>
    public record UserbyNameRequest(string Username);


    /// <summary>
    /// Запрос пользователя по Id
    /// </summary>
    /// <param name="Id">Id пользователя</param>
    public record UserbyIdRequest(string Id);


    /// <summary>
    /// Модель ответа на запрос пользователя
    /// </summary>
    /// <param name="Id">Id пользователя</param>
    /// <param name="Username">имя пользователя</param>
    /// <param name="HashPassword">хэш пароля пользователя</param>
    public record UserResponse(string Id, string Username, string HashPassword);
}
