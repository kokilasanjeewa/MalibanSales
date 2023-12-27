using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class DeleteSupplierProductDto
  {
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
  }
}