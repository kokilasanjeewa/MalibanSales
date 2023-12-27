using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class ReturnReasonListDto
  {
    [Required]
    public int ReturnTypeId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The MethodType cannot exceed 50 characters. ")]
    public string ReturnType { set; get; }
    [StringLength(100, ErrorMessage = "The PaymentDescription cannot exceed 100 characters. ")]
    public string ReturnDescription { set; get; }
  }
}