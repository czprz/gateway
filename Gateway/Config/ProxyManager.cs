using Gateway.Yarp;

namespace Gateway.Config;

public class ProxyManager : IProxyManager
{
    private readonly IYarpFacade _yarpFacade;

    public ProxyManager(IYarpFacade yarpFacade)
    {
        _yarpFacade = yarpFacade;
    }

    public IReadOnlyList<RouteConfig> GetRoutes()
    {
        return _yarpFacade.Read();
    }

    public void AddRoute(RouteConfig route)
    {
        _yarpFacade.Update(route);
        // TODO: Save DB
    }

    public void RemoveRoute(string route)
    {
        // TODO: Remove from Yarp and DB
    }
}