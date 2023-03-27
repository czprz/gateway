namespace Gateway.Components.Routing.Models;

public class RouteConfigDto
{
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public string? Path { get; init; }
    public IReadOnlyList<QueryParameterDto>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderDto>? Headers { get; init; }
    
    public bool? AuthenticationRequired { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Audience { get; init; }
    public string? Scopes { get; init; }

    public IReadOnlyList<ProxyDto> Proxies { get; init; }
}