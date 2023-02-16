using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobyLabWebProgramming.Infrastructure.Configurations;
using MobyLabWebProgramming.Infrastructure.Database;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobyLabWebProgramming.Infrastructure.Converters;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Workers;
using Serilog;
using Serilog.Events;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Repositories.Implementation;

namespace MobyLabWebProgramming.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddRepository(this WebApplicationBuilder builder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddDbContext<WebAppDatabaseContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("WebAppDatabase"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                    .CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds)));
        builder.Services.AddTransient<IRepository<WebAppDatabaseContext>, Repository<WebAppDatabaseContext>>();

        return builder;
    }

    public static WebApplicationBuilder AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        var corsConfiguration = builder.Configuration.GetSection(nameof(CorsConfiguration)).Get<CorsConfiguration>();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policyBuilder =>
                {
                    policyBuilder.WithOrigins(corsConfiguration.Origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        return builder;
    }

    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
            .AddMvc()
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        return builder;
    }

    private static AuthorizationPolicyBuilder AddDefaultPolicy(this AuthorizationPolicyBuilder policy) =>
        policy.RequireClaim(ClaimTypes.NameIdentifier)
            .RequireClaim(ClaimTypes.Name)
            .RequireClaim(ClaimTypes.Email);

    private static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtConfiguration = builder.Configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();

                var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key);
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
            }).Services
            .AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().AddDefaultPolicy().Build();
            });

        return builder;
    }

    public static WebApplicationBuilder AddAuthorizationWithSwagger(this WebApplicationBuilder builder, string application)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<SmartEnumSchemaFilter>();
            c.SwaggerDoc("v1", new() { Title = application, Version = "v1" });
            c.AddSecurityDefinition("Bearer", new()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder.ConfigureAuthentication();
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
        builder.Services.Configure<FileStorageConfiguration>(builder.Configuration.GetSection(nameof(FileStorageConfiguration)));
        builder.Services
            .AddTransient<IUserService, UserService>()
            .AddTransient<ILoginService, LoginService>()
            .AddTransient<IFileRepository, FileRepository>()
            .AddTransient<IUserFileService, UserFileService>();

        return builder;
    }

    public static WebApplicationBuilder UseLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, logger) =>
        {
            logger
                .MinimumLevel.Is(LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .WriteTo.Console();
        });

        return builder;
    }

    public static WebApplicationBuilder AddWorkers(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<InitializerWorker>();

        return builder;
    }
}
