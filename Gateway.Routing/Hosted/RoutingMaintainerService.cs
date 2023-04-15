using Gateway.Common.Config;
using Gateway.Routing.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gateway.Routing.Hosted;

public class RoutingMaintainerService : IHostedService
{
    private readonly IRoutingRepository _routingRepository;
    private readonly IConfig _config;
    private readonly ILogger<RoutingMaintainerService> _logger;
    
    private static Timer? _timer;
    
    public RoutingMaintainerService(IRoutingRepository routingRepository, IConfig config, ILogger<RoutingMaintainerService> logger)
    {
        _routingRepository = routingRepository;
        _config = config;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_ => CheckRoutes(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_config.CheckIntervalSeconds));
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        
        return Task.CompletedTask;
    }

    private async void CheckRoutes()
    {
        var routes = await _routingRepository.Get();
        foreach (var route in routes)
        {
            if (route.CreatedAt < DateTime.Now.AddSeconds(_config.InactiveTimeoutSeconds))
            {
                continue;
            }

            if (route.UpdatedAt.HasValue && !(route.UpdatedAt < DateTime.Now.AddSeconds(-_config.InactiveTimeoutSeconds)))
            {
                continue;
            }
            
            _logger.LogDebug("Route {RouteId: 0} has not been updated for 30 minutes, therefore route has now been deleted", route.Id.ToString());
            
            await _routingRepository.Remove(route.Id);
        }
    }
}