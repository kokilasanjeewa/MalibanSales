using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UserForConfirmEmailDto
  {
    [Required]
    public string Token { get; set; }
    [Required]
    public string UserId { get; set; }


  }
}