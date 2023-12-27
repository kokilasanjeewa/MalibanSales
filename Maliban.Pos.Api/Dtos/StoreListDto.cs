using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class StoreListDto
  {
    [Required]
    public int StoreId { get; set; }
    [Required]
    public string StoreName { get; set; }
    [Required]
    public string BranchLocationName { get; set; }
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
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
  }
}