using Gateway.Config;

namespace Gateway.Yarp;

public interface IYarpFacade
{
    void Update(RouteConfig route);
    IReadOnlyList<Config.RouteConfig> Read();
}