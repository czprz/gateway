using Gateway.Components.Auth.Endpoints;
using Gateway.Components.Auth.Exchanges;
using Gateway.Components.Auth.Handlers;
using Gateway.Components.Auth.Services;
using Gateway.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TokenHandler = Gateway.Components.Auth.Handlers.TokenHandler;

namespace Gateway.Components.Auth;

public static class AuthFlowExtension
{
    public static async void AddAuthFlow(this WebApplicationBuilder builder)
    {
        var config = builder.Services.BuildServiceProvider().GetRequiredService<IConfig>();

        // builder.Services.AddHttpClient("discovery-endpoint", client =>
        // {
        //     client.BaseAddress = new(config.Authority);
        // });

        builder.Services.AddTokenExchangeService(config);

        // builder.Services.AddTransient<IDiscoveryService, DiscoveryService>();
        // Handlers
        builder.Services.AddTransient<ITokenHandler, TokenHandler>();
        builder.Services.AddTransient<ILogoutHandler, LogoutHandler>();
        
        // Token Services
        builder.Services.AddTransient<ITokenRefreshService, TokenRefreshService>();
        builder.Services.AddTransient<IApiTokenService, ApiTokenService>();
        builder.Services.AddTransient<ITokenService, TokenService>();

        builder.Services.AddDistributedMemoryCache();

        var sessionTimeoutInMin = config.SessionTimeoutInMin;
        builder.Services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromMinutes(sessionTimeoutInMin); });

        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy("authPolicy", policy => { policy.RequireAuthenticatedUser(); });
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(setup =>
            {
                setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutInMin);
                setup.SlidingExpiration = true;
            })
            .AddOpenIdConnect(opt =>
            {
                opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.Authority = config.Authority;
                opt.ClientId = config.ClientId;
                opt.UsePkce = true;
                opt.ClientSecret = config.ClientSecret;
                opt.ResponseType = OpenIdConnectResponseType.Code;
                opt.SaveTokens = false;
                opt.GetClaimsFromUserInfoEndpoint = config.QueryUserInfoEndpoint;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.RequireHttpsMetadata = false;

                var scopes = config.Scopes;
                var scopeArray = scopes.Split(" ");
                foreach (var scope in scopeArray)
                {
                    opt.Scope.Add(scope);
                }

                opt.Events.OnTokenValidated = context =>
                {
                    var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();
                    tokenHandler.HandleToken(context);
                    return Task.CompletedTask;
                };

                opt.Events.OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var logoutHandler = context.HttpContext.RequestServices.GetRequiredService<ILogoutHandler>();
                    logoutHandler.Logout(context);
                    return Task.CompletedTask;
                };
            });
    }

    public static void UseAuthFlow(this WebApplication app)
    {
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        app.AddAuthEndpoints();
    }

    private static void AddTokenExchangeService(this IServiceCollection services, IConfig config)
    {
        var strategy = config.TokenExchangeStrategy;

        switch (strategy.ToLower())
        {
            case "none":
            {
                services.AddSingleton<ITokenExchangeService, NoTokenExchangeService>();
                break;
            }
        }
    }
}