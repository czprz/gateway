using Asp.Versioning.Builder;
using AutoMapper;
using Gateway.Components.Routing.Models;
using Gateway.Components.Routing.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Components.Routing.Endpoints;

public static class RoutingEndpoints
{
    public static void AddRoutingEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/routes", GetRoutes)
            .WithTags("Routing")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
        app.MapPost("api/routes", AddRoute)
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
        var routes = proxyManager.GetRoutes();

        return routes.Count == 0 ? Results.NotFound() : Results.Ok(proxyManager.GetRoutes());
    }

    private static async Task<IResult> AddRoute([FromBody] RouteConfigDto routeConfigDto, IProxyManager proxyManager, IMapper mapper)
    {
        try
        {
            var route = mapper.Map<RouteConfigDto, RouteConfig>(routeConfigDto);
            proxyManager.AddRoute(route);
            
            return Results.Ok();
        }
        catch (Exception ex)
        {
            // TODO: Better exception handling and logging
            return Results.BadRequest("Failed to add route");
        }
    }

    private static async Task<IResult> RemoveRoute(string route, IProxyManager proxyManager)
    {
        try
        {
            proxyManager.RemoveRoute(route);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            // TODO: Better exception handling and logging
            return Results.NotFound();
        }
    }
}