using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

public class HubConnectionService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly NavigationManager _navigation;
    private readonly AuthService _authService;

    public HubConnectionService(
        NavigationManager navigation,
        AuthService authService)
    {
        _navigation = navigation;
        _authService = authService;
    }

    public HubConnection GetConnection()
    {
        _hubConnection ??= new HubConnectionBuilder()
            .WithUrl(_navigation.ToAbsoluteUri("/chatHub"), options =>
            {
                options.AccessTokenProvider = async () =>
                    await _authService.GetTokenAsync();
            })
            .WithAutomaticReconnect()
            .Build();

        return _hubConnection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}