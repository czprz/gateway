using AutoMapper;
using Gateway.Config;
using Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Endpoints;

public static class RoutingEndpoints
{
    public static void AddRoutingEndpoints(this WebApplication app)
    {
        app.MapGet("/api/routes", GetRoutes);
        app.MapPost("api/routes", AddRoute);
        app.MapDelete("api/routes/{route}", RemoveRoute);
    }

    private static async Task<IResult> GetRoutes(IProxyManager proxyManager)
    {
        var routes = proxyManager.GetRoutes();

        return routes.Count == 0 ? Results.NotFound() : Results.Ok(proxyManager.GetRoutes());
    }

    private static async Task<IResult> AddRoute([FromBody] RouteDefinition routeDefinition, IProxyManager proxyManager, IMapper mapper)
    {
        try
        {
            var route = mapper.Map<RouteDefinition, RouteConfig>(routeDefinition);
            proxyManager.AddRoute(route);
            
            return Results.Ok();
        }
        catch (Exception ex)
        {
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
            return Results.NotFound();
        }
    }
}