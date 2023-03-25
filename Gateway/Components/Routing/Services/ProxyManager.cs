namespace Gateway.Components.Routing.Services;

public class ProxyManager : IProxyManager
{
    private readonly IYarpFacade _yarpFacade;
    private readonly IRoutingRepository _routingRepository;

    public ProxyManager(IYarpFacade yarpFacade, IRoutingRepository routingRepository)
    {
        _yarpFacade = yarpFacade;
        _routingRepository = routingRepository;
    }

    public IReadOnlyList<RouteConfig> GetRoutes()
    {
        return _yarpFacade.Read();
    }

    public void AddRoute(RouteConfig route)
    {
        _yarpFacade.Update(route);
        _routingRepository.Save(route);
    }

    public void RemoveRoute(string route)
    {
        // TODO: Remove from Yarp and DB
    }
}