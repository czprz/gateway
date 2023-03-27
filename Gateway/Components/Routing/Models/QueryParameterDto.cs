namespace Gateway.Components.Routing.Models;

public class QueryParameterDto
{
    public string Name { get; init; } = "";
    public IReadOnlyList<string>? Values { get; init; }
    public QueryParameterMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}