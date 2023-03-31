using System.Collections.ObjectModel;
using Asp.Versioning.Conventions;
using Gateway.Components.Auth;
using Gateway.Components.Routing;
using Gateway.Config;
using Gateway.Swagger;
using Yarp.ReverseProxy.Configuration;
using RouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GatewayConfig>(
    builder.Configuration.GetSection("GatewayConfig"));

builder.Services.AddReverseProxy()
    .LoadFromMemory(new Collection<RouteConfig>(), new Collection<ClusterConfig>());

builder.Services.AddSingleton<IConfig, Config>();

builder.Services.AddHealthChecks();

builder.AddSwagger();

builder.AddRoutingService();
builder.AddAuthFlow();

var app = builder.Build();

app.MapReverseProxy(p => p.AddAuthFlowTokenPipe());

app.MapHealthChecks("/healthz");

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(1, 0)
    .Build();

app.UseSwagger();

// Must be placed above UseAuthFlow
app.UseRouting();

app.UseRoutingService(versionSet);
app.UseAuthFlow();

app.Run();