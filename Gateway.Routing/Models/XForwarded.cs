namespace Gateway.Routing.Models;

public class XForwarded
{
    public string Action { get; init; } = "";
    public string? For { get; init; }
    public string? Proto { get; init; }
    public string? Host { get; init; }
    public string? Prefix { get; init; }
    public string? HeaderPrefix { get; init; }
}