using Microsoft.AspNetCore.Authorization;

namespace Maliban.Pos.Api.Helper
{
  public class MinimumAgeRequirement : IAuthorizationRequirement
  {
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
      MinimumAge = minimumAge;
    }
  }
}