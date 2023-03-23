namespace Gateway.Models;

public class RouteDefinition
{
    public IReadOnlyList<string>? Methods { get; init; }
    public IReadOnlyList<string>? Hosts { get; init; }
    public string? Path { get; init; }
    public IReadOnlyList<QueryParameterDefinition>? QueryParameters { get; init; }
    public IReadOnlyList<HeaderDefinition>? Headers { get; init; }
    
    public IReadOnlyList<Proxy> Proxy { get; init; }
}

public class Proxy
{
    public string Address { get; init; }
    public string HealthProbeAddress { get; init; }
}


public class QueryParameterDefinition
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public QueryParameterMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}

public enum QueryParameterMatchMode
{
    Exact,
    Contains,
    NotContains,
    Prefix,
    Exists
}

public class HeaderDefinition
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}

public enum HeaderMatchMode
{
    ExactHeader,
    HeaderPrefix,
    Contains,
    NotContains,
    Exists,
    NotExists,
}