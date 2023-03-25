namespace Gateway.Components.Routing.Services;

public class RouteConfig
{
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public string? Path { get; init; }
    public IReadOnlyList<QueryParameterMatch>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderMatch>? Headers { get; init; }
    
    public bool? AuthenticationRequired { get; init; }
    
    public IReadOnlyList<Proxy> Proxy { get; init; }
}

public class Proxy
{
    public string Address { get; init; }
    public string HealthProbeAddress { get; init; }
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