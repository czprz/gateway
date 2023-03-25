using System.Collections.ObjectModel;
using Gateway.Components.Auth;
using Gateway.Components.Routing;
using Gateway.Config;
using Yarp.ReverseProxy.Configuration;
using RouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GatewayConfig>(
    builder.Configuration.GetSection("GatewayConfig"));

builder.Services.AddReverseProxy()
    .LoadFromMemory(new Collection<RouteConfig>(), new Collection<ClusterConfig>());

builder.Services.AddSingleton<IConfig, Config>();

builder.AddRoutingService();
builder.AddAuthFlow();

var app = builder.Build();

app.MapReverseProxy();

// Must be placed above UseAuthFlow
app.UseRouting();

app.UseRoutingService();
app.UseAuthFlow();

app.Run();