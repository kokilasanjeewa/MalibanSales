using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateRackDto
  {
    [Required]
    public string RackName { set; get; }
    [Required]
    public string RackDescription { set; get; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    [Required]
    public int? StoreAreaId { get; set; }
  }
}