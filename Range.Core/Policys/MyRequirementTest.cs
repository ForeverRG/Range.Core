using Microsoft.AspNetCore.Authorization;

namespace Range.Core.Policys
{
  public class MyRequirementTest : IAuthorizationRequirement
  {
    public string Name { get; set; }
    public string Sex { get; set; }
    public int Age { get; set; } 
  }
}
