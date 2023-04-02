namespace Gateway.Routing.Models;

public class RouteConfig
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string? Path { get; init; }
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public IReadOnlyList<QueryParameterMatch>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderMatch>? Headers { get; init; }
    
    public bool? UseAuthentication { get; init; }
    
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Audience { get; init; }
    public string? Scopes { get; init; }
    
    public LoadBalancingPolicy? LoadBalancingPolicy { get; init; }

    public IReadOnlyList<Upstream> Upstreams { get; init; }
}