using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class ReturnReason : BaseModel
  {
    [Key]
    public int ReturnTypeId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The MethodType cannot exceed 50 characters. ")]
    public string ReturnType { set; get; }
    [StringLength(100, ErrorMessage = "The PaymentDescription cannot exceed 100 characters. ")]
    public string ReturnDescription { set; get; }
    [Required]
    public string ReturnTypeUid { get; set; } = Guid.NewGuid().ToString();

  }
}