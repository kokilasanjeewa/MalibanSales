using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePhotoUploadDto
  {
    [NotMapped]
    public IFormFile Files { get; set; }
    [NotMapped]
    public string PhotoDescription { set; get; }
  }
}