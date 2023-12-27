using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateSupplierPriceDto
    {
        [Required]
        [ConcurrencyCheck]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "CostPrice")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Profit")]
        public decimal Profit { get; set; } = 0;
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "MrpPrice")]
        public decimal MaximumRetailPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Discount Price")]
        public decimal DiscountPrice { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Discount Precentage")]
        public decimal DiscountPrecentage { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Promotion Price")]
        public decimal PromotionPrice { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Promotion Precentage")]
        public decimal PromotionPrecentage { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Tax Precentage")]
        public decimal? TaxPrecentage { get; set; } = 0;
        [Required]
        public StatusType StatusType { get; set; } = StatusType.Add;
        [Required]
        public Int64 ProductId { get; set; }
        [Required]
        public int BranchLocationId { get; set; }
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PriceDate { get; set; }
        public string CreatedBy { get; set; }

    }
}