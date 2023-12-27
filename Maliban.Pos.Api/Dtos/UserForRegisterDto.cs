using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class UserForRegisterDto
  {
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }
    public string NationId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string SurName { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public string Address { get; set; }
    public string BirthPlace { get; set; }
    public string BirthTime { get; set; }
    public string ProfileImageURL { get; set; }
    public string ContactNumber { get; set; }
    [Required]
    public string UserName { get { return Email; } }
    [Required]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 20 characters.")]
    public string Password { get; set; }
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    [Required]
    public int BranchLocationId { get; set; }

  }
}