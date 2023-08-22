namespace Gateway.Routing.Models;

public class Forwarded
{
    public string Values { get; init; } = "";
    public string? ForFormat { get; init; }
    public string? ByFormat { get; init; }
    public string? Action { get; init; }
}