using System.Collections.ObjectModel;
using Asp.Versioning.Conventions;
using Gateway.Components.Auth;
using Gateway.Components.Routing;
using Gateway.Config;
using Gateway.Swagger;
using Yarp.ReverseProxy.Configuration;
using RouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfig();

builder.Services.AddReverseProxy()
    .LoadFromMemory(new Collection<RouteConfig>(), new Collection<ClusterConfig>());

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