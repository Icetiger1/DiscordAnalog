using AutoMapper;
using DiscordAnalog.Server.Core.Interfaces;
using DiscordAnalog.Server.Infrastructure.Data;
using DiscordAnalog.Server.Infrastructure.Repositories;
using DiscordAnalog.Server.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DiscordAnalog.Server.API.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary> Добавляет БД с поддержкой разных провайдеров </summary>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                // Можно легко заменить на SQL Server, SQLite и т.д.
                options.UseNpgsql(config.GetConnectionString("PostgreSQL"));
            });

            return services;
        }

        /// <summary> Регистрация Swagger </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiscordAnalog API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>  Регистрация аутентификации </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                    };
                });

            return services;
        }

        /// <summary> Добавляет кастомные сервисы </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Репозитории
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();

            // Сервисы
            services.AddScoped<ITokenService, TokenService>();

            // Автомаппер
            // 1. Создаем логгер фабрику (можно использовать системную)
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            // 2. Регистрируем конфигурацию
            services.AddSingleton<AutoMapper.IConfigurationProvider>(sp =>
                new MapperConfiguration(
                    cfg => cfg.AddProfile<MappingProfile>(), // Action<IMapperConfigurationExpression>
                    loggerFactory // ILoggerFactory
                ));

            // 3. Регистрируем IMapper
            services.AddScoped<IMapper>(sp =>
                new Mapper(
                    sp.GetRequiredService<AutoMapper.IConfigurationProvider>(),
                    sp.GetService // Функция разрешения зависимостей
                ));


            // Регистрация AutoMapper (корректно для v15+)
            //services.AddSingleton<AutoMapper.IConfigurationProvider>(_ =>
            //    new MapperConfiguration(
            //        cfg => cfg.AddProfile<MappingProfile>(),
            //        new NullLoggerFactory() // Microsoft.Extensions.Logging.Abstractions
            //));

            return services;
        }
    }
}
