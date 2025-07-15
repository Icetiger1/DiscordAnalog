using Microsoft.AspNetCore.SignalR;

namespace DiscordAnalog.Server.API.Hubs
{
    /// <summary>
    /// VoiceHub
    /// </summary>
    public class VoiceHub : Hub
    {
        /// <summary>
        /// Отправка SDP оффера от одного клиента другому
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <param name="offer"></param>
        /// <returns></returns>
        public async Task SendOffer(string targetUserId, string offer)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveOffer",
                Context.UserIdentifier, offer);
        }

        /// <summary>
        /// Отправка SDP ответа
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task SendAnswer(string targetUserId, string answer)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveAnswer",
                Context.UserIdentifier, answer);
        }

        /// <summary>
        /// Обмен ICE-кандидатами
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public async Task SendIceCandidate(string targetUserId, string candidate)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveIceCandidate",
                Context.UserIdentifier, candidate);
        }
    }
}
