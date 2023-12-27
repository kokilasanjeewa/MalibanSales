using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseReturnDetail
  {
    [Key]
    public Int64 PurchaseReturnDetailId { get; set; }
    [Required]
    public string PurchaseReturnDetailUid { get; set; } = Guid.NewGuid().ToString();
    // [Required]
    //  public Int64 PurchaseOrderHeaderId { get; set; }
    [Required]
    public Int64 PurchaseReturnHeaderId { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    /*     [Required]
        [StringLength(100, ErrorMessage = "The Barcode cannot exceed 100 characters. ")]
        public string Barcode { get; set; } */
    //  [Required]
    //   public int OrderQuantity { get; set; }
    [Required]
    public decimal ReturnQuantity { get; set; }
    /*     [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Price")]
        public decimal OrderPrice { get; set; } */
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Return Price")]
    public decimal ReturnPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Amount")]
    public decimal SubTotal { get; set; }
    public decimal FreeQuantity { get; set; }
    public virtual PurchaseReturnHeader PurchaseReturnHeader { get; set; }
    public virtual Product Product { get; set; }
    [ForeignKey("PriceId")]
    public virtual Price Price { get; set; }    
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifiedDate { get; set; }
    // [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
    [StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
    public string ModifiedBy { get; set; }
    public bool Active { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    [Required]
    public Int64 TransactionDetailId { get; set; }
    [ForeignKey("TransactionDetailId")]
    public virtual TransactionDetail TransactionDetail { get; set; }
    }
}