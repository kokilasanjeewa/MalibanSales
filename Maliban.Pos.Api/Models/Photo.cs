using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Photo : BaseModel
  {
    [Key]
    public Int64 PhotoId { set; get; }
    [StringLength(450, ErrorMessage = "The Url cannot exceed 100 characters. ")]
    public string Url { set; get; }
    [StringLength(200, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { set; get; }
    [Display(Name = "Photo Added")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public bool IsMain { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "The AttachmentPath cannot exceed 200 characters. ")]
    public string AttachmentPath { get; set; }
    [Required]
    public long Length { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The ContentType cannot exceed 50 characters. ")]
    public string ContentType { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The FileName cannot exceed 100 characters. ")]
    public string FileName { get; set; }
  }
}