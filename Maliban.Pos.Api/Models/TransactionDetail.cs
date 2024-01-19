using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class TransactionDetail : BaseModel
  {
    [Key]
    public Int64 TransactionDetailId { get; set; }
    public string TransactionDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public decimal dQuantity { get; set; }
    public bool ProductAvailable { get; set; } = true;
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    public virtual Price Price { get; set; }
    public Int64 iDocID { get; set; }
    [StringLength(50, ErrorMessage = "The cDocNo cannot exceed 50 characters. ")]
    public string cDocNo { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime dDocDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    [StringLength(5, ErrorMessage = "The cDocType cannot exceed 5 characters. ")]
    public cDocType cDocType { get; set; }
    [StringLength(20, ErrorMessage = "The cExtRefNum cannot exceed 20 characters. ")]
    public string cExtRefNum { get; set; }
    [Required]
    public decimal dIssueQuantity { get; set; } = 0;
    [Required]
    public decimal dNetQuantity { get; set; } = 0;
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
   [ConcurrencyCheck]
    public decimal dCostPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [ConcurrencyCheck]
    public decimal dSalePrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [ConcurrencyCheck]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal dValue { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [ConcurrencyCheck]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal dNetValue { get; set; }
    [Required]
    public decimal dIN { get; set; }
    [Required]
    public decimal dOUT { get; set; }
    [Column("Barcode", Order = 3, TypeName = "nvarchar(100)")]
    public string Barcode { get; set; }
    [Required]
    [Column("ProductName", Order = 4, TypeName = "nvarchar(100)")]
    public string ProductName { get; set; }
    [NotMapped]
    public decimal dPreNetQuantity { get; set; } = 0;
    public ICollection<SaleDetail> SaleDetails { get; set; }

    }
    public enum cDocType { GRN, INV, AIN,AOUT, PRN, SLR,STO,STI }
}




