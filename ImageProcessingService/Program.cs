using System.Text;
using ImageProcessingService.Context;
using ImageProcessingService.Misc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ImageProcessingService;

public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        //authorize button in swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });

        
        builder.Services.AddSingleton<IJWT, JWT>();
        builder.Services.AddSingleton<IHash, Hash>();
        builder.Services.AddSingleton<ITransform, Transform>();
            
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:7088",
                ValidAudience = "https://localhost:7088",
                IssuerSigningKey = new SymmetricSecurityKey("6291e317333c686986923f74ab8874b19d97638e57f2b09d6323c8a365d85817d01794223fd17bfcccd466a99d37cd8b7e76bc1fba4ae4a3a8f5c9fdb6a377145d4c197bb0a771217caa5645cfdf7a626ca6a2ad733e3a6e9c3cc813af9cf9f60f7cae9fef1e9725a6f6d0799605bdbca3af4e837b41111048399e46545c947ec3e702c77b8b0a222b24f4b4a6db2bf4fbb4369d801da58abd14e8e8a294ae16d6a569d1d25e29a02c7799e5d395c8daced2aaea0d8fb6eb3059c4f279a3d6bacb3d1a299d7743b062288e2498e7a3e0aa0f5c95b2287af15c39ce24028f759cfd67a6a474bc4a5856282cb5d47cc659b57ff4b6e2b4a679715db777c4f38f3d"u8.ToArray())
            };
        });
        
        var requireAuthPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(requireAuthPolicy);
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();        
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}