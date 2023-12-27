using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateStoreDto
  {

    [Required]
    public string StoreName { get; set; }
    [Required]
    public string StoreDescription { get; set; }
    [Required]
    public int? BranchLocationId { get; set; }
    [Required]
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