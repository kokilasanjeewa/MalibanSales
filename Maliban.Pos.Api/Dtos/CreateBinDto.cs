using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateBinDto
  {
    [Required]
    public int? BranchLocationId { get; set; }
    [Required]
    public int? BinNumber { get; set; }
    [Required]
    public int? StoreId { set; get; }
    [Required]
    public int? StoreAreaId { set; get; }
    [Required]
    public int? ShelfNumber { set; get; }
    [Required]
    public int? RackId { get; set; }
    [Required]
    public string BinDescription { set; get; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    public string CreatedBy { get; set; }

  }
}