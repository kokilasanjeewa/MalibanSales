using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class SavePurchaseOrderHeaderDto
  {
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public int SupplierId { get; set; }
  }
}