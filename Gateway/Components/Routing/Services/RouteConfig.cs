namespace Gateway.Components.Routing.Services;

public class RouteConfig
{
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public string? Path { get; init; }
    public IReadOnlyList<QueryParameterMatch>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderMatch>? Headers { get; init; }
    
    public bool? AuthenticationRequired { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Audience { get; init; }
    public string? Scopes { get; init; }

    public IReadOnlyList<Proxy> Proxies { get; init; }
}

public class Proxy
{
    public string Address { get; set; } = "";
    public string? HealthProbeAddress { get; set; }
}

public class QueryParameterMatch
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public QueryParameterMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}

public enum QueryParameterMatchMode
{
    Exact = 0,
    Contains = 1,
    NotContains = 2,
    Prefix = 3,
    Exists = 4
}

public class HeaderMatch
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}

public enum HeaderMatchMode
{
    ExactHeader = 0,
    HeaderPrefix = 1,
    Contains = 2,
    NotContains = 3,
    Exists = 4,
    NotExists = 5,
}