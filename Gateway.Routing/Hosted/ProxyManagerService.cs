using Gateway.Routing.Services;
using Microsoft.Extensions.Hosting;

namespace Gateway.Routing.Hosted;

public class ProxyManagerService : IHostedService
{
    private readonly IYarpFacade _yarpFacade;
    private readonly IProxyFacade _proxyFacade;

    public ProxyManagerService(IYarpFacade yarpFacade, IProxyFacade proxyFacade)
    {
        _yarpFacade = yarpFacade;
        _proxyFacade = proxyFacade;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await UpdateRoutes();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task UpdateRoutes()
    {
        var routes = await _proxyFacade.Get();
        _yarpFacade.Update(routes);
    }
}