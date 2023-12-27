using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UserForSendConfirmEmailDto
  {
    [Required]
    public string UserName { get; set; }
  }
}