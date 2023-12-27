using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class BranchLocation : BaseModel
  {
    [Key]
    public int BranchLocationId { get; set; }
    public string BranchLocationUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(4, MinimumLength = 2, ErrorMessage = "The BranchCode cannot exceed 4 characters. ")]
    public string BranchCode { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "The BranchName cannot exceed 60 characters. ")]
    public string BranchName { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "The Address cannot exceed 200 characters. ")]
    public string Address { get; set; }
    [Required]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "The ContactNumbers cannot exceed 15 characters. ")]
    public string ContactNumbers { get; set; }
    [StringLength(50, ErrorMessage = "The Longitude cannot exceed 50 characters. ")]
    public string Longitude { get; set; }
    [StringLength(50, ErrorMessage = "The Longitude cannot exceed 50 characters. ")]
    public string Latitude { get; set; }
    public virtual ICollection<Store> Store { get; set; }
    public virtual ICollection<StoreArea> StoreAreas { get; set; }

    public virtual ICollection<Stock> Stock { get; set; }
    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }
    public virtual Country Country { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public virtual ICollection<Supplier> Suppliers { get; set; }
    public ICollection<SupplierProduct> SupplierProducts { get; set; }
    public ICollection<GoodReceptNoteHeader> GoodReceptNoteHeaders { get; set; }

  }
}