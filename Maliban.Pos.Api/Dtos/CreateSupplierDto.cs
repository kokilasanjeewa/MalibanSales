using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateSupplierDto
  {
    [Required]
    [Display(Name = "Supplier Company Name")]
    public string SupplierName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public int? TitleId { get; set; }
    [Required]
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    [Required]
    public int? CityId { get; set; }
    [Required]
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public int? PostalCode { get; set; }
    public string Phone { get; set; }
    [Required]
    public string Mobile { get; set; }
    public string Fax { get; set; }
    public string Email { get; set; }
    public string URL { get; set; }
    public string DiscountType { get; set; }
    public string Notes { get; set; }
    public bool DiscountAvailable { get; set; }
    public string Logo { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    public IFormFile Files { get; set; }
    public bool Active { get; set; } = true;
    public int? ProvinceId { get; set; }
    public int? TypeofGoodId { get; set; }
    public int? PaymentMethodId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public string BRNO { get; set; }
  }
}