using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("TransformValues")]
public class TransformValuesDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Key { get; set; }
    public string Value { get; set; }
}