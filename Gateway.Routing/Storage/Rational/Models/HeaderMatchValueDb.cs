using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Routing.Storage.Rational.Models;

[Table("HeaderMatchValues")]
public class HeaderMatchValueDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Value { get; set; } = "";
}