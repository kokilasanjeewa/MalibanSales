using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class RemoveSaleItemQuantityDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public int quantity { get; set; }
    [Required]
    public int branchLocationId { get; set; }
    
  }
}