using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Range.Core.Policys
{
  public class MyRequirementHandler : AuthorizationHandler<MyRequirementTest>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirementTest requirement)
    {
      if (requirement.Age > 10)
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }
      return Task.CompletedTask;
    }
  }
}
