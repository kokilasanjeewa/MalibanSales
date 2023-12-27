using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePaymentMethodDto
  {
    [Required]
    [StringLength(50, ErrorMessage = "The MethodType cannot exceed 50 characters. ")]
    public string MethodType { set; get; }
    [StringLength(100, ErrorMessage = "The PaymentDescription cannot exceed 100 characters. ")]
    public string PaymentDescription { set; get; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreateBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}