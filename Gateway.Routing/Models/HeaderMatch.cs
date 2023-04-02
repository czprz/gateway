namespace Gateway.Routing.Models;

public class HeaderMatch
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}