using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yarp.Config;
using Yarp.Config.Mapping;
using Yarp.Models;

namespace Yarp.Endpoints;

public static class RoutingEndpoints
{
    public static void AddRoutingEndpoints(this WebApplication app)
    {
        app.MapGet("/api/routes", GetRoutes);
        app.MapPost("api/route", AddRoute);
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

    private static async Task<IResult> GetRoutes(IProxyManager proxyManager)
    {
        var routes = proxyManager.GetRoutes();

        return routes.Count == 0 ? Results.NotFound() : Results.Ok(proxyManager.GetRoutes());
    }
}