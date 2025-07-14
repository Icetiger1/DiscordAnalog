using AutoMapper;
using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Core.DTOs;
using DiscordAnalog.Server.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DiscordAnalog.Server.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        public ChatHub(IChatRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        /// <summary> Подключение к каналу + загрузка истории </summary>
        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");

            var messages = await _roomRepo.GetMessagesAsync(roomId, 100);
            var dtos = _mapper.Map<List<MessageDto>>(messages);

            await Clients.Caller.SendAsync("ReceiveMessagesHistory", dtos);
        }

        /// <summary> Отправка нового сообщения </summary>
        public async Task SendMessage(int roomId, string text)
        {
            var userId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var message = new Message
            {
                Content = text,
                UserId = userId,
                ChatRoomId = roomId,
                Timestamp = DateTime.UtcNow
            };

            await _roomRepo.AddMessageAsync(roomId, message);

            var dto = _mapper.Map<MessageDto>(message);
            dto.Username = Context.User!.Identity!.Name!;

            await Clients.Group($"room-{roomId}").SendAsync("ReceiveNewMessage", dto);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Логика при отключении (необязательно)
            await base.OnDisconnectedAsync(exception);
        }
    }
}
