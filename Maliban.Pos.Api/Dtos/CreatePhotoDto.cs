using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePhotoDto
  {
    public string Url { set; get; }
    public string Description { set; get; }
    public bool IsMain { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    [Required]
    public string AttachmentPath { get; set; }
    [Required]
    public long Length { get; set; }
    [Required]
    public string ContentType { get; set; }
    [Required]
    public string FileName { get; set; }
  }
}