using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class DeleteSupplierPriceDto
  {
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
  }
}