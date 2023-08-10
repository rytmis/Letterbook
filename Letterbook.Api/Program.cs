using System.Net.Mime;
using System.Text;
using Letterbook.Adapter.Db;
using Letterbook.Adapter.RxMessageBus;
using Letterbook.Core;
using Letterbook.Core.Adapters;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Letterbook.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register controllers
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SnakeCaseRouteTransformer()));
        });
        
        // Register config
        var coreOptions = builder.Configuration.GetSection(CoreOptions.ConfigKey);
        builder.Services.Configure<CoreOptions>(coreOptions);
        builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.ConfigKey));
        builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(DbOptions.ConfigKey));
        
        // Register Services
        builder.Services.AddScoped<IActivityService, ActivityService>();
        builder.Services.AddScoped<IActivityEventService, ActivityEventService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IAccountEventService, AccountEventService>();
        builder.Services.AddScoped<IAccountProfileAdapter, AccountProfileAdapter>();
        
        // Register Adapters
        builder.Services.AddScoped<IActivityAdapter, ActivityAdapter>();
        builder.Services.AddSingleton<IMessageBusAdapter, RxMessageBus>();
        builder.Services.AddDbContext<TransactionalContext>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Authorization header, including the Bearer scheme, like so: `Bearer <JWT>`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
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

        var opts = coreOptions.Get<CoreOptions>() 
                   ?? throw new ArgumentException("Invalid configuration", nameof(CoreOptions));
        var apiOpts = coreOptions.Get<ApiOptions>() 
                   ?? throw new ArgumentException("Invalid configuration", nameof(ApiOptions));
        builder.WebHost.UseUrls(CoreOptions.BaseUri(opts).ToString());
        
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                var secretBytes = Encoding.UTF8.GetBytes(apiOpts.OAuthSecret);
                var key = new SymmetricSecurityKey(secretBytes);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = CoreOptions.BaseUri(opts).ToString(),
                    ValidateAudience = true,
                    ValidAudiences = new[] { CoreOptions.BaseUri(opts).ToString() },
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key
                };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UsePathBase(new PathString("/api/v1"));


        app.MapControllers();

        app.Run();
    }
}
