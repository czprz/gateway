namespace Gateway.Routing.Endpoints.Models;

public class RouteConfigDto
{
    // TODO: Write validator, must have at least one selection condition
    public string? Path { get; init; }
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public IReadOnlyList<QueryParameterDto>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderDto>? Headers { get; init; }
    
    /// <summary>
    /// Use for increasing the max request body size
    /// </summary>
    // TODO: Check what happens if higher than long.MaxValue
    public long? MaxRequestBodySize { get; init; }
    
    /// <summary>
    /// Use if you want to use authentication for this route
    /// </summary>
    public bool? UseAuthentication { get; init; }
    
    /// <summary>
    /// Use for token exchange
    /// </summary>
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Audience { get; init; }
    public string? Scopes { get; init; }
    
    // TODO: Improve this
    public IReadOnlyList<IReadOnlyDictionary<string, string>>? Transforms { get; init; }
    
    /// <summary>
    /// Use to choose the load balancing policy (default is PowerOfTwoChoices)
    /// </summary>
    public LoadBalancingPolicy? LoadBalancingPolicy { get; init; }
    
    // TODO: Add validator, must have at least one upstream
    /// <summary>
    /// Use for sending requests to the upstream and load balancing
    /// </summary>
    public IReadOnlyList<UpstreamDto> Upstreams { get; init; }
}