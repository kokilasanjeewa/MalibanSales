using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class CardtMethod : BaseModel
    {
        [Key]
        public int CardMethodId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The MethodType cannot exceed 50 characters. ")]
        public string CardName { set; get; }
        [StringLength(100, ErrorMessage = "The PaymentDescription cannot exceed 100 characters. ")]
        public string CardDescription { set; get; }
        [Required]
        public string CardMethodUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ServiceCharge { get; set; } = 0;
        [Required]
        public int PaymentMethodId { get; set; }
        [Required]
        public int BranchLocationId { get; set; }

    }
}