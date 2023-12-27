using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePurchaseRequestHeaderDto
  {
    [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public int? DepartmentId { get; set; }
    [Required]
    public int? SectionId { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpectedDate { get; set; }
  }
}