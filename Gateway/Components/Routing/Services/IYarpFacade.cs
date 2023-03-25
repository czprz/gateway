namespace Gateway.Components.Routing.Services;

public interface IYarpFacade
{
    void Update(RouteConfig route);
    IReadOnlyList<RouteConfig> Read();
}