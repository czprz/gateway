namespace Gateway.Routing.Endpoints.Models;

public class HeaderDto
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}