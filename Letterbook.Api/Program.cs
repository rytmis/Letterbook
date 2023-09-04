using System.Text.Json;
using System.Text.Json.Serialization;
using Letterbook.Adapter.Db;
using Letterbook.Adapter.RxMessageBus;
using Letterbook.Adapter.TimescaleFeeds;
using Letterbook.Core;
using Letterbook.Core.Adapters;
using Letterbook.Core.Extensions;
using Letterbook.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using OpenIddict.Validation.AspNetCore;

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
        }).AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        
        // Register OIDC
        builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<RelationalContext>();
            })
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetTokenEndpointUris("connect/token");

                // Enable the client credentials flow.
                options.AllowAuthorizationCodeFlow();

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
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
        builder.Services.AddDbContext<RelationalContext>();
        builder.Services.AddDbContext<FeedsContext>();
        builder.Services.AddIdentity<AccountAuthentication, IdentityRole>()
            .AddUserManager<AccountAuthentication>()
            .AddSignInManager<AccountAuthentication>();
        
        // TODO: Move to db adapter
        // services.AddDbContext<ApplicationDbContext>(options =>
        // {
        //     // Configure Entity Framework Core to use Microsoft SQL Server.
        //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        //
        //     // Register the entity sets needed by OpenIddict.
        //     // Note: use the generic overload if you need to replace the default OpenIddict entities.
        //     options.UseOpenIddict();
        // });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var opts = coreOptions.Get<CoreOptions>() 
                   ?? throw new ArgumentException("Invalid configuration", nameof(CoreOptions));
        builder.WebHost.UseUrls(opts.BaseUri().ToString());
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
