using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Maliban.Pos.Api.Models
{
  public class ApplicationUser : IdentityUser<int>
  {
    public ApplicationUser() : base() { }
    public string Uid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50, MinimumLength = 12, ErrorMessage = "The Role description cannot exceed 50 characters. ")]
    public override string UserName { get; set; }
    [Required]
    [Column(Order = 3)]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 20 characters.")]
    public string Password { get; set; }
    [Column(Order = 5)]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
    [Column(Order = 6)]
    [StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
    public string ModifiedBy { get; set; }
    [Column(Order = 7)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RegDate { get; set; } = DateTime.Now;
    [Column("SystemNumber", Order = 9)]
    [StringLength(100, ErrorMessage = "The SystemNumber cannot exceed 100 characters. ")]
    public string SystemNumber { get; set; }
    [Column(Order = 18)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifiedDate { get; set; }
    [Column(Order = 18)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastAccessDate { get; set; }
    [Column(Order = 20)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastConnectedDate { get; set; }
    [Column(Order = 21)]
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastLoginDate { get; set; }
    [Column(Order = 33)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }
    [StringLength(100, ErrorMessage = "The NationId cannot exceed 100 characters. ")]
    public string NationId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The FirstName cannot exceed 50 characters. ")]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The LastName cannot exceed 50 characters. ")]
    public string LastName { get; set; }
    [StringLength(50, ErrorMessage = "The MiddleName cannot exceed 50 characters. ")]
    public string MiddleName { get; set; }
    [StringLength(50, ErrorMessage = "The SurName cannot exceed 50 characters. ")]
    public string SurName { get; set; }
    [StringLength(200, ErrorMessage = "The Address cannot exceed 200 characters. ")]
    public string Address { get; set; }
    [StringLength(100, ErrorMessage = "The Address cannot exceed 100 characters. ")]
    public string BirthPlace { get; set; }
    [StringLength(400, ErrorMessage = "The Address cannot exceed 400 characters. ")]
    public string ProfileImageURL { get; set; }
    [StringLength(50, ErrorMessage = "The ContactNumber cannot exceed 50 characters. ")]
    public string ContactNumber { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    [Required]
    public int? BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    public virtual ICollection<SaleHeader> SaleHeader { get; set; }
    [StringLength(50, ErrorMessage = "The Address cannot exceed 400 characters. ")]
    public override string PhoneNumber { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }
    public virtual Country Country { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public string IpAddress { get; set; }
    public bool Active { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
  }
}