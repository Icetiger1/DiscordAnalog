using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.API.Hubs;


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


// 2. ��������� ����������
// --------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// �����: UseAuthentication �� UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();