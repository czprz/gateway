using System.Collections.ObjectModel;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Yarp.Config;
using Yarp.Config.Mapping;
using Yarp.Endpoints;

using YarpRouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;
using YarpClusterConfig = Yarp.ReverseProxy.Configuration.ClusterConfig;

var builder = WebApplication.CreateBuilder(args);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapFromDefinitionToRouteConfig>();
    cfg.AddProfile<MapFromRouteConfigToYarpRouteConfig>();
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IProxyManager, ProxyManager>();

builder.Services.AddReverseProxy()
    .LoadFromMemory(new Collection<YarpRouteConfig>(), new Collection<YarpClusterConfig>());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost/core"; // Replace with your OAuth provider URL
        options.Audience = "gateway"; // Replace with your app's audience name
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