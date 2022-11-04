using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Metflix.BL.MediatR.QueryHandlers.Movies;
using Metflix.DL.Seeding;
using Metflix.DL.Seeding.Contracts;
using Metflix.Host.Extensions;
using Metflix.Host.HealthChecks;
using Metflix.Host.Middleware.ErrorHandlerMiddleware;
using Metflix.Models.Configurations;
using Metflix.Models.DbModels.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Metflix.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddSerilog(logger);
            builder.Services
                .AddHealthChecks()
                .AddCheck<SqlHealthCheck>("SQL Server");

            // Register Configurations

            builder.Services.Configure<ConnectionStrings>(
                builder.Configuration.GetSection(nameof(ConnectionStrings)));
            builder.Services.Configure<Jwt>(
                builder.Configuration.GetSection(nameof(Jwt)));

            //ADD JWT Authentication

            builder.Services.AddSwaggerGen(x =>
            {
                // ADD Jwt Token
                var jwtSecurityScheme = new OpenApiSecurityScheme()
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token in the textbox below",
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    },
                };

                x.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                     {jwtSecurityScheme,Array.Empty<string>() }
                 });
            });

            // ADD Jwt Token
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<IDbSeeder, DbSeeder>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMediatR(typeof(GetAllMoviesQueryHandler).Assembly);
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

            //Register Service Extensions

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            using (var serviceScope = app.Services.CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetRequiredService<IDbSeeder>();
                await seeder.SeedDb();
            }

            app.RegisterHealthChecks();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}