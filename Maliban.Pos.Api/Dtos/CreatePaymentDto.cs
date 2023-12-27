using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePaymentDto
  {

    public PaymentType PaymentType { get; set; }
    public bool Allowed { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreateBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}