using System.Text;
using api.Configuration;
using api.Data;
using api.Filters;
using api.Interfaces;
using api.Model;
using api.Repository;
using api.Seeders.Data;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureProjectServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddControllers(options =>
        {
            options.Filters.Add<ApiResponseFilter>();
        });

        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });
            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
        });
        service.AddScoped<IStockRepository, StockRepository>();
        service.AddScoped<ICommentRepository, CommentRepository>();
        service.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        service.AddScoped<ITokenService, TokenService>();
        service.AddScoped<IPortfolioRepository, PortfolioRepository>();


        string databseUrl = configuration.GetConnectionString("Default")!;
        Console.WriteLine($"[STARTUP] Database URL retrieved: {(string.IsNullOrWhiteSpace(databseUrl) ? "NO (Null or empty)" : "YES (Found)")}");
        
        service.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(databseUrl);

            options.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var dbContext = (ApplicationDbContext)context;
                var userManager = dbContext.GetService<UserManager<AppUser>>();
                await RoleSeeder.SeedAsync(dbContext, cancellationToken);
                await AppUserSeeder.SeedAsync(userManager, cancellationToken);
                await StockSeeder.SeedAsync(dbContext, cancellationToken);
                await CommentSeeder.SeedAsync(dbContext, cancellationToken);
                await PortFolioSeeder.SeedAsync(dbContext, cancellationToken);
                await AdminUserSeeder.SeedAsync(userManager, cancellationToken);
            });

            options.UseSeeding((context, _) =>
            {
                var dbContext = (ApplicationDbContext)context;
                var userManager = dbContext.GetService<UserManager<AppUser>>();
                RoleSeeder.SeedAsync(dbContext, CancellationToken.None).GetAwaiter().GetResult();
                AppUserSeeder.SeedAsync(userManager, CancellationToken.None).GetAwaiter().GetResult();
                StockSeeder.SeedAsync(dbContext, CancellationToken.None).GetAwaiter().GetResult();
                CommentSeeder.SeedAsync(dbContext, CancellationToken.None).GetAwaiter().GetResult();
                PortFolioSeeder.SeedAsync(dbContext, CancellationToken.None).GetAwaiter().GetResult();
                AdminUserSeeder.SeedAsync(userManager, CancellationToken.None).GetAwaiter().GetResult();
            });
        });



        service.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        Console.WriteLine("[STARTUP] Retrieving JWT Configuration...");
        var jwtSection = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        if (jwtSection == null)
        {
            Console.WriteLine("[FATAL CRASH] JwtConfiguration is completely missing from environment variables.");
            throw new Exception("Value not found.");
        }
        else
        {
            Console.WriteLine($"[STARTUP] JWT Config loaded. Issuer: {jwtSection.Issuer}, Audience: {jwtSection.Audience}");
        }
        JwtConfiguration jwtConfiguration = jwtSection;



        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey))
            };
        });

        service.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
        });

        return service;
    }
}
