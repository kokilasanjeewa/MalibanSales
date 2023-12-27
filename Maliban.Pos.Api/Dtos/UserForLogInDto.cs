using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UserForLogInDto
  {
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Please enter Email")]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6,
         ErrorMessage = "Password Should be minimum 6 characters and a maximum of 20 characters")]
    public string Password { get; set; }
  }
}