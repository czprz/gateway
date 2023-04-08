using Asp.Versioning.Conventions;
using AutoMapper;
using Gateway.Auth;
using Gateway.Auth.Maps;
using Gateway.Common.Config;
using Gateway.Routing;
using Gateway.Routing.Maps;
using Gateway.Routing.Storage.Rational.Maps;
using Gateway.Swagger;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfig();

var mapConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<UserInfoMaps>();
    cfg.AddProfile<RouteConfigDbMaps>();
    cfg.AddProfile<RouteConfigMaps>();
    cfg.AddProfile<YarpRouteConfigMaps>();
    cfg.AddProfile<YarpClusterConfigMaps>();
});

var mapper = mapConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddReverseProxy()
    .LoadFromMemory(new List<RouteConfig>(), new List<ClusterConfig>());

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