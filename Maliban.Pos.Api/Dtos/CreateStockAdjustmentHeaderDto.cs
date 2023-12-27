using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateStockAdjustmentHeaderDto
  {
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "Order Date")]
    public DateTime StockTakenDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    //public DateTime PaymentDate {  get{return DateTime.Now; } set{} }
    public DateTime StockAdjustmentDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    [Required]
    public int AdjustmentTypeId { get; set; }
    [Required]
    [Display(Name = "BranchLocation Id")]
    public int BranchLocationId { get; set; }
    [Required]
    [Display(Name = "ApplicationUser Id")]
    public int Id { get; set; }
    //[Required]
    public ICollection<StockAdjustmentDetail> Lines { get; set; }
    public string CreatedBy { get; set; }
  }
}