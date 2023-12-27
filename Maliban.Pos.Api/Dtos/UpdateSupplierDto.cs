using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateSupplierDto
  {
    [Required]
    [Display(Name = "Supplier Company Name")]
    [StringLength(100, ErrorMessage = "The Supplier Name cannot exceed 100 characters. ")]
    public string SupplierName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The FirstName Name cannot exceed 50 characters. ")]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The LastName Name cannot exceed 50 characters. ")]
    public string LastName { get; set; }
    /* [Required]
     [StringLength(5, ErrorMessage = "The Title Name cannot exceed 5 characters. ")]
     public string Title { get; set; } */
    [Required]
    [StringLength(150, ErrorMessage = "The Address1  cannot exceed 100 characters. ")]
    public string Address1 { get; set; }
    [StringLength(15, ErrorMessage = "The Title Name cannot exceed 100 characters. ")]
    public string Phone { get; set; }
    [StringLength(15, ErrorMessage = "The Title Name cannot exceed 100 characters. ")]
    public string Fax { get; set; }
    [StringLength(1000, ErrorMessage = "The Title Name cannot exceed 100 characters. ")]
    public string Email { get; set; }
    [StringLength(400, ErrorMessage = "The Title Name cannot exceed 100 characters. ")]
    public string URL { get; set; }
    public string DiscountType { get; set; }
    [StringLength(200, ErrorMessage = "The Notes cannot exceed 200 characters. ")] public string Notes { get; set; }
    public bool DiscountAvailable { get; set; } = false;
    [StringLength(400, ErrorMessage = "The Title Name cannot exceed 400 characters. ")]
    public string LogoUrl { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public int? TitleId { get; set; }
    public int? ProvinceId { get; set; }
    public int? TypeofGoodId { get; set; }
    public int? PaymentMethodId { get; set; }
    public bool Active { get; set; } = true;
    public string BRNO { get; set; }
  }
}