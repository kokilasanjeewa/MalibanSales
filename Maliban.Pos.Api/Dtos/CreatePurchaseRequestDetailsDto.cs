using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePurchaseRequestDetailsDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public int quantity { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string createdBy { get; set; }

  }
}