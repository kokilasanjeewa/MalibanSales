using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class BrandListDto
  {
    [Required]
    public int BrandId { get; set; }
    [Required]
    public string BrandName { set; get; }
    public string BrandDescription { set; get; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool Active { get; set; }
  }
}