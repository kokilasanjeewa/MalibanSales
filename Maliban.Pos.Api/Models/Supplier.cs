using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  // [Table("Supplier", Schema = "pos")]
  public class Supplier : BaseModel
  {
    [Key]
    public int SupplierId { get; set; }
    [Required]
    public string SupplierUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Supplier Company Name")]
    [StringLength(100, ErrorMessage = "The Supplier Name cannot exceed 100 characters. ")]
    public string SupplierName { get; set; }
    [Display(Name = "BR Number")]
    [StringLength(100, ErrorMessage = "The Business Registration Number cannot exceed 100 characters. ")]
    public string BRNO { get; set; }
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
    [StringLength(150, ErrorMessage = "The Address2 cannot exceed 100 characters. ")]
    public string Address2 { get; set; }
    [StringLength(150, ErrorMessage = "The Address3 cannot exceed 100 characters. ")]
    public string Address3 { get; set; }
    [StringLength(15, ErrorMessage = "The Title Name cannot exceed 100 characters. ")]
    public string Phone { get; set; }
    [StringLength(15, ErrorMessage = "The mobile number cannot exceed 100 characters. ")]
    public string Mobile { get; set; }
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
    [JsonIgnore]
    public ICollection<SupplierPrice> SupplierPrice { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }
    public virtual Country Country { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public virtual Title Title { get; set; }
    public int? TitleId { get; set; }
    public virtual Province Provinces { get; set; }
    public int? ProvinceId { get; set; }
    public int? TypeofGoodId { get; set; }
    public int? PaymentMethodId { get; set; }
    public virtual TypeofGood TypeofGood { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; }
    public ICollection<SupplierProduct> SupplierProducts { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    //public virtual ICollection<StockTransferHeader> StockTransferHeaders { get; set; }

    }
}
