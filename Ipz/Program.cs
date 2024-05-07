using Ipz.Middlewares;
using Ipz.Services;
using Ipz.Services.IServices;
using Ipz_server.Models.Database;
using Ipz_server.Services;
using Ipz_server.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using System.Reflection;
using System.Security.Cryptography;

namespace Ipz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FoodDeliveryContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddHttpContextAccessor();



            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();


            var publicKey = builder.Configuration.GetValue<string>("JWT:PublicKey");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                RSA rsa = RSA.Create();
                rsa.FromXmlString(publicKey);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Example: Bearer <YourTokenHere>",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
           .WriteTo.Console()
           .CreateLogger();

            builder.Logging.AddSerilog().SetMinimumLevel(LogLevel.Information);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AuthMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
