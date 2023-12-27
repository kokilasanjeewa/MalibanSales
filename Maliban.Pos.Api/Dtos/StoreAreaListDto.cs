using System;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class StoreAreaListDto
  {
    [Required]
    public int StoreAreaId { get; set; }
    [Required]
    public string StoreAreaName { set; get; }
    [Required]
    public string StoreAreaDescriptions { set; get; }
    public bool IsDeleted { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifyDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    [Display(Name = "Modified By")]
    public string ModifiedBy { get; set; }
  }
}