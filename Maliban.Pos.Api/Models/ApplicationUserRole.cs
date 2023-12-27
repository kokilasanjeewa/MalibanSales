using Microsoft.AspNetCore.Identity;

namespace Maliban.Pos.Api.Models
{
  public class ApplicationUserRole : IdentityUserRole<int>
  {
    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual ApplicationRole ApplicationRole { get; set; }
  }
}

