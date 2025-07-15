using DiscordAnalog_Blazor_Client.Service;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static DiscordAnalogModelsClassLibrary.Core.DTOs.AuthDto;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly LocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public AuthService(
        HttpClient http,
        LocalStorageService localStorage,
        NavigationManager navigation)
    {
        _http = http;
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("/api/auth/login",
            new AuthRequest(username, password));

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            await _localStorage.SetItemAsync("jwtToken", result!.Token);
            return true;
        }
        return false;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("/api/auth/register",
            new AuthRequest(username, password));

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            await _localStorage.SetItemAsync("jwtToken", result!.Token);
            return true;
        }
        return false;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync("jwtToken");
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("jwtToken");
        _navigation.NavigateTo("/login");
    }
}