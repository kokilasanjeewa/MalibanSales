using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class ApplicationFiles
  {
    [Key]
    public int IFileId { get; set; }
    public string Uid { get; set; } = Guid.NewGuid().ToString();
    public string AttachmentPath { get; set; }
    public string UrlPath { get; set; }
    public byte[] Attachment { get; set; }
    public bool Delete { get; set; }
    public string ContentType { get; set; }
    public string Name { get; set; }
    public long Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy H:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
  
  }
}