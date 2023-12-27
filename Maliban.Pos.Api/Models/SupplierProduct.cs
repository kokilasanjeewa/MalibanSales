using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class SupplierProduct
  {
    [Required]
    public Int64 ProductId { get; set; }
    public Product Product { get; set; }
    [Required]
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public BranchLocation BranchLocation { get; set; }

  }
}