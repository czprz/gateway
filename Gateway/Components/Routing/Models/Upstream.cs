namespace Gateway.Components.Routing.Models;

public class Upstream
{
    public string Address { get; set; } = "";
    public string? HealthProbeAddress { get; set; }
}