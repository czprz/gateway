namespace Gateway.Components.Routing.Models;

public class RouteDto
{
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public string? Path { get; init; }
    public IReadOnlyList<QueryParameterDto>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderDto>? Headers { get; init; }
    
    public bool? AuthenticationRequired { get; init; }
    
    public IReadOnlyList<ProxyDto> Proxy { get; init; }
}