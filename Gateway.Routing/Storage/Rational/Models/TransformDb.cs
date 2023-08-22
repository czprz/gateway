using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("Transforms")]
public class TransformDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public TransformTypeDb Type { get; set; }
    
    public ICollection<TransformValuesDb> Values { get; set; }
}

public enum TransformTypeDb
{
    PathPrefix,
    PathRemovePrefix,
    PathSet,
    XForwarded,
    Forwarded
}

public static class TransformTypeDbExtensions
{
    public static string ToKey(this TransformTypeDb type)
    {
        return type switch
        {
            TransformTypeDb.PathPrefix => "PathPrefix",
            TransformTypeDb.PathRemovePrefix => "PathRemovePrefix",
            TransformTypeDb.PathSet => "PathSet",
            TransformTypeDb.XForwarded => "X-Forwarded",
            TransformTypeDb.Forwarded => "Forwarded",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}