using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateStoreAreaDto
  {

    [Required]
    public string StoreAreaName { get; set; }
    [Required]
    public string StoreAreaDescription { get; set; }
    [Required]
    public int? BranchLocationId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    [Required]
    public int? StoreId { get; set; }

  }
}