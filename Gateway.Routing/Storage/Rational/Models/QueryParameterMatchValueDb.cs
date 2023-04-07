using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("QueryParameterMatchValues")]
public class QueryParameterMatchValueDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Value { get; set; } = "";
}