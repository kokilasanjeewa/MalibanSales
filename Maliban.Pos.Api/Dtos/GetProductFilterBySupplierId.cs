using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class GetProductFilterBySupplierId
    {
        [Required]
        public Int64 ProductId { get; set; }
        [Required]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
    }
}