using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class PaymentMethod : BaseModel
  {
    [Key]
    public int PaymentMethodId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The MethodType cannot exceed 50 characters. ")]
    public string MethodType { set; get; }
    [StringLength(100, ErrorMessage = "The PaymentDescription cannot exceed 100 characters. ")]
    public string PaymentDescription { set; get; }

    [Required]
    public string PaymentMethodUid { get; set; } = Guid.NewGuid().ToString();


  }
}