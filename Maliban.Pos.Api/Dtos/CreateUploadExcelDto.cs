using Microsoft.AspNetCore.Http;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateUploadExcelDto
  {
    public IFormFile Files { get; set; }

  }
}