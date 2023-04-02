using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public class RationalDbRoutingStorage : IRoutingRepository
{
    public IReadOnlyList<RouteConfig> Get()
    {
        throw new NotImplementedException();
    }

    public RouteConfig? Get(string key)
    {
        throw new NotImplementedException();
    }

    public void Save(RouteConfig route)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }
}