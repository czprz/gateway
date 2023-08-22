namespace Gateway.Routing.Models;

public class RouteConfig
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int? MatchHashCode { get; set; }

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
    
    public Transforms? Transforms { get; init; }

    public LoadBalancingPolicy? LoadBalancingPolicy { get; init; }

    public IReadOnlyList<Upstream> Upstreams { get; init; }

    public int GetMatchHash()
    {
        var hash = 17;
        hash = hash * 23 + Path?.GetHashCode() ?? 0;
        hash = (Methods ?? Array.Empty<string>()).Aggregate(hash,
            (current, method) => current * 23 + method.GetHashCode());
        hash = (Hosts ?? Array.Empty<string>()).Aggregate(hash, (current, host) => current * 23 + host.GetHashCode());
        hash = (QueryParameters ?? Array.Empty<QueryParameterMatch>()).Aggregate(hash,
            (current, queryParameter) => current * 23 + queryParameter.GetHashCode());

        return (Headers ?? Array.Empty<HeaderMatch>()).Aggregate(hash,
            (current, header) => current * 23 + header.GetHashCode());
    }
}

public class Transforms
{
    public RequestTransform? RequestTransform { get; set; }
}

public class RequestTransform
{
    public string? PathPrefix { get; set; }
    public string? PathRemovePrefix { get; set; }
    public string? PathSet { get; set; }
    public XForwarded? XForwarded { get; set; }
    public Forwarded? Forwarded { get; set; }
}