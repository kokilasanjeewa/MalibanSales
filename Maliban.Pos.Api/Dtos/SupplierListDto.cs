using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class SupplierListDto
  {
    [Required]
    public int SupplierId { get; set; }
    [Required]
    [Display(Name = "Supplier Company Name")]
    public string SupplierName { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address1 { get; set; }
    public int PostalCode { get; set; }
    public int Phone { get; set; }
    public bool IsDeleted { get; set; }
    public string DistrictName { get; set; }
    public string TitleName { get; set; }
    public string ProvinceName { get; set; }
    public string CityName { get; set; }
    public string Fax { get; set; }
    public string Email { get; set; }
    public string[] Photos { get; set; }

  }
}