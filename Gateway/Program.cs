using AutoMapper;
using Gateway.Config;
using Gateway.Config.Maps;
using Gateway.Endpoints;
using Gateway.Yarp;
using Gateway.Yarp.Maps;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapFromDefinitionToRouteConfig>();
    cfg.AddProfile<MapFromRouteConfigToYarpRouteConfig>();
    cfg.AddProfile<MapFromRouteConfigToYarpClusterConfig>();
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.AddReverseProxy();

builder.Services.AddTransient<IProxyManager, ProxyManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("AUTHORITY__ADDRESS");
        options.Audience = Environment.GetEnvironmentVariable("AUTHORITY__AUDIENCE");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicy", policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();

app.MapReverseProxy();

app.AddRoutingEndpoints();
app.UseRouting();

app.UseAuthentication();

app.Run();