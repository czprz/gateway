using Asp.Versioning.Builder;
using AutoMapper;
using Gateway.Routing.Endpoints.Models;
using Gateway.Routing.Models;
using Gateway.Routing.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Routing.Endpoints;

public static class RoutingEndpoints
{
    public static void AddRoutingEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        // TODO: Endpoints should only be accessible for upstream services
        app.MapGet("/api/routes", GetRoutes)
            .WithTags("Routing")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
        app.MapPut("api/routes", UpdateRoutes)
            .WithTags("Routing")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
        app.MapPost("api/routes", AddRoutes)
            .WithTags("Routing")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
        app.MapDelete("api/routes/{route}", RemoveRoute)
            .WithTags("Routing")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetRoutes(IProxyManager proxyManager)
    {
        var routes = await proxyManager.Get();

        return routes.Count == 0 ? Results.NotFound() : Results.Ok(routes);
    }

    private static async Task<IResult> AddRoutes([FromBody] RouteConfigDto routeConfigDto, IProxyManager proxyManager, IMapper mapper)
    {
        var route = mapper.Map<RouteConfigDto, RouteConfig>(routeConfigDto);

        return await proxyManager.Add(route) switch
        {
            ProxyManagerResult.Error => Results.StatusCode(StatusCodes.Status500InternalServerError),
            ProxyManagerResult.AlreadyExists => Results.Conflict(),
            _ => Results.Ok()
        };
    }

    private static async Task<IResult> UpdateRoutes([FromBody] RouteConfigDto routeConfigDto, IMapper mapper, IProxyManager proxyManager)
    {
        var route = mapper.Map<RouteConfigDto, RouteConfig>(routeConfigDto);

        return await proxyManager.Update(route) switch
        {
            ProxyManagerResult.Error => Results.StatusCode(StatusCodes.Status500InternalServerError),
            ProxyManagerResult.NotFound => Results.NotFound(),
            _ => Results.Ok()
        };
    }

    private static async Task<IResult> RemoveRoute(string route, IProxyManager proxyManager)
    {
        return await proxyManager.Remove(route) switch
        {
            ProxyManagerResult.Error => Results.StatusCode(StatusCodes.Status500InternalServerError),
            ProxyManagerResult.NotFound => Results.NotFound(),
            _ => Results.Ok()
        };
    }
}