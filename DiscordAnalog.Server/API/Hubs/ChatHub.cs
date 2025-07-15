using AutoMapper;
using DiscordAnalogModelsClassLibrary.Core.DTOs;
using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DiscordAnalog.Server.API.Hubs
{
    /// <summary>
    /// ChatHub
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomRepo"></param>
        /// <param name="mapper"></param>
        public ChatHub(IChatRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        /// <summary> 
        /// Подключение к каналу + загрузка истории 
        /// </summary>
        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");

            var messages = await _roomRepo.GetMessagesAsync(roomId, 100);
            var dtos = _mapper.Map<List<MessageDto>>(messages);

            await Clients.Caller.SendAsync("ReceiveMessagesHistory", dtos);
        }

        /// <summary>
        /// Отправка нового сообщения 
        /// </summary>
        public async Task SendMessage(string roomId, string text)
        {
            var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var message = new Message
            {
                Content = text,
                UserId = userId,
                ChatRoomId = roomId,
                Timestamp = DateTime.UtcNow
            };

            await _roomRepo.AddMessageAsync(int.Parse(roomId), message);

            var dto = _mapper.Map<MessageDto>(message);
            dto.Username = Context.User!.Identity!.Name!;

            await Clients.Group($"room-{roomId}").SendAsync("ReceiveNewMessage", dto);
        }

        /// <summary>
        /// Логика при отключении (необязательно)
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
