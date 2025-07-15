using DiscordAnalogModelsClassLibrary.Core.DTOs;
using System.Net.Http.Json;


public class ChatService
{
    private readonly HttpClient _http;

    public ChatService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ChannelDto>> GetChannelsAsync()
    {
        return await _http.GetFromJsonAsync<List<ChannelDto>>("/api/channels") ?? new();
    }

    public async Task<List<MessageDto>> GetChannelMessagesAsync(string channelId)
    {
        return await _http.GetFromJsonAsync<List<MessageDto>>($"/api/channels/{channelId}/messages") ?? new();
    }
}

