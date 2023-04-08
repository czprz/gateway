using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("RouteConfigs")]
public class RouteConfigDb
{
    [Key]
    public Guid Id { get; set; }
    public string? Path { get; set; }
    public ICollection<MethodDb> Methods { get; set; }
    public ICollection<HostDb> Hosts { get; set; }
    public ICollection<QueryParameterMatchDb> QueryParameters { get; set; }
    public ICollection<HeaderMatchDb> Headers { get; set; }
    
    public bool? UseAuthentication { get; set; }
    
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Audience { get; set; }
    public string? Scopes { get; set; }
    
    public LoadBalancingPolicyDb? LoadBalancingPolicy { get; set; }
    
    public ICollection<UpstreamDb> Upstreams { get; set; }
}