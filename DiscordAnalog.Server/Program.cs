using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.API.Hubs;
using DiscordAnalog.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 1. ���������� �������� � ��������� DI
// --------------------------------------------------
// ����� ������� ����������
builder.Services
    .AddDatabaseContext(builder.Configuration) //DataBase
    .AddSwagger()                // Swagger
    .AddJwtAuthentication(builder.Configuration) // JWT
    .AddApplicationServices();   // ������� ����������

// ���������� ��������� ��������
builder.Services.AddControllers();
builder.Services.AddSignalR();

// ��������� CORS (��� Blazor WASM)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://your-client-address:port")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
});


// 2. ��������� ����������
// --------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

// �����: UseAuthentication �� UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

// ��������� SignalR
app.MapHub<ChatHub>("/chatHub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});

// ��������� ���������� �������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services.GetRequiredService<AppDbContext>());
}


app.Run();