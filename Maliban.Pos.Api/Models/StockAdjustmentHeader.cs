using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class StockAdjustmentHeader : BaseModel
  {
    [Key]
    public Int64 StockAdjustmentHeaderId { get; set; }
    [Required]
    public string StockAdjustmentHeaderUid { get; set; } = Guid.NewGuid().ToString();
    //[Required]
    [StringLength(100, ErrorMessage = "The Stock Adjustment Name cannot exceed 100 characters. ")]
    public string StockAdjustmentsName { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StockTakenDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StockAdjustmentDate { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    public bool IsProcess { get; set; } = false;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "StockAdjustmentCostAmount")]
    [Required]
    public decimal StockAdjustmentCostAmount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "StockAdjustmentSaleAmount")]
    [Required]
    public decimal StockAdjustmentSaleAmount { get; set; }
    public virtual AdjustmentType AdjustmentType { get; set; }
    public ICollection<StockAdjustmentDetail> Lines { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public int AdjustmentTypeId { get; set; }
    [Required]
    [Display(Name = "ApplicationUser Id")]
    public int Id { get; set; }
    [ForeignKey("Id")]
    public virtual ApplicationUser ApplicationUser { get; set; }
    [Required]
    public bool IsApprove { get; set; } = false;
    }

}