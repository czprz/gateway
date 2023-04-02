using System.ComponentModel.DataAnnotations;

namespace Gateway.Routing.Endpoints.Models;

public class UpstreamDto
{
    [Required] 
    public string Address { get; init; } = "";

    public string? HealthProbeAddress { get; init; }
}