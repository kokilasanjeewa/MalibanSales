using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Maliban.Pos.Api.Models
{
  public class ApplicationRole : IdentityRole<int>
  {
    public string RoleName { get; set; }
    public string RoleIcon { get; set; }
    public bool IsActive { get; set; }
    public ICollection<RolePermission> Permissions { get; set; }
    public ApplicationRole(string roleName, string description,
    DateTime createdDate)
    : base(roleName)
    {
      base.Name = roleName;

      this.Description = description;
      this.CreatedDate = createdDate;
    }
    //[StringLength(50, MinimumLength = 10, ErrorMessage="Remark must have min length of 10 and max Length of 50")]
    [StringLength(50, ErrorMessage = "The Role description cannot exceed 50 characters. ")]
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }
  }
}