using System;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.DataAccess
{
  public class DataContextP : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
  {
    public DataContextP(DbContextOptions<DataContextP> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<ApplicationUserRole>(userRole =>
      {
        userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
        userRole.HasOne(ur => ur.ApplicationRole).WithMany(r => r.ApplicationUserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
        userRole.HasOne(ur => ur.ApplicationUser).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
      });

    }


  }
}