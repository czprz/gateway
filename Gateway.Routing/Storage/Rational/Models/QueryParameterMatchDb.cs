using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("QueryParameterMatches")]
public class QueryParameterMatchDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public IEnumerable<QueryParameterMatchValueDb>? Values { get; set; }
}