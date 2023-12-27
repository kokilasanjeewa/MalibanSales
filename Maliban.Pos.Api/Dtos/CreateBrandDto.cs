using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateBrandDto
  {
    [Required]
    public string BrandName { set; get; }
    public string BrandDescription { set; get; }
    [Required]
    public string CreatedBy { get; set; }

  }
}