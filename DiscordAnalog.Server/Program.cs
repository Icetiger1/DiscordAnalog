using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.API.Hubs;


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


// 2. Настройка приложения
// --------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Важно: UseAuthentication ДО UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();