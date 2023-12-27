using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class SuppliersDto
  {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        [Display(Name = "Supplier Company Name")]
        public string SupplierName { get; set; }
  }
}