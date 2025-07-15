using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.API.Hubs;
using DiscordAnalog.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 1. Добавление сервисов в контейнер DI
// --------------------------------------------------
// Вызов методов расширения
builder.Services
    .AddDatabaseContext(builder.Configuration) //DataBase
    .AddSwagger()                // Swagger
    .AddJwtAuthentication(builder.Configuration) // JWT
    .AddApplicationServices();   // Сервисы приложения

// Добавление остальных сервисов
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Настройка CORS (для Blazor WASM)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://your-client-address:port")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
});


// 2. Настройка приложения
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

// Важно: UseAuthentication ДО UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

// Настройка SignalR
app.MapHub<ChatHub>("/chatHub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});

// Заполняем первичными данными
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services.GetRequiredService<AppDbContext>());
}


app.Run();