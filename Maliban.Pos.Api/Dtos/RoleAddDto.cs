using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class RoleAddDto
  {
    [Required]
    public string[] RoleNames { get; set; }

    }
}