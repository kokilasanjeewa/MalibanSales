using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class MealOrderDetail : BaseModel
    {
        [Key]
        [Display(Name = "Order Detail Id")]
        public Int64 MealOrderDetailId { get; set; }
        [Required]
        public string MealOrderDetailUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Display(Name = "Product Id")]
        public Int64 ProductId { get; set; }
        [Required]
        [Display(Name = "Price Id")]
        public Int64 PriceId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("PriceId")]
        public virtual Price Price { get; set; }
        [Required]
        public Int64 MealOrderHeaderId { get; set; }
        [ForeignKey("MealOrderHeaderId")]
        public virtual MealOrderHeader MealOrderHeader { get; set; }



    }
}