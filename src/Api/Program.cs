using Data.Repositories;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Service.Authentication;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração de CORS mais permissiva para debug
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins([
                        "http://localhost:3000",
                        "http://192.168.1.4:3000",
                        "https://localhost:7240",
                        "http://localhost:3001", // Adicione outras portas se necessário
                        "https://localhost:3000"
                    ])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidAudience = builder.Configuration["AppSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name,
                        ClockSkew = TimeSpan.Zero // Remove tolerância de tempo
                    };

                    // Eventos para debug
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine($"JWT Challenge: {context.Error}, {context.ErrorDescription}");
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                });

            // DbContext
            builder.Services.AddDbContext<DefaultDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            // Services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Scalar OpenAPI
            builder.Services.AddOpenApi();

            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tcc-Api", Version = "v1" });

                // JWT Suporte no Swagger
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Informe seu token JWT no formato: Bearer {seu token}",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Find.Collect");
                    c.RoutePrefix = string.Empty;
                });

                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
                db.Database.Migrate();
            }

            app.UseCors("AllowFrontend");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
