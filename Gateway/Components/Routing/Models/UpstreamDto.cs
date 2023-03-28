using System.ComponentModel.DataAnnotations;

namespace Gateway.Components.Routing.Models;

public class UpstreamDto
{
    [Required] 
    public string Address { get; init; } = "";

    public string? HealthProbeAddress { get; init; }
}