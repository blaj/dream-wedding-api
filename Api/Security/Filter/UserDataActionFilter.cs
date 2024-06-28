using System.Security.Claims;
using DreamWeddingApi.Api.Security.DTO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DreamWeddingApi.Api.Security.Filter;

public class UserDataActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.ContainsKey("userData"))
        {
            return;
        }

        var userClaims = context.HttpContext.User.Claims.ToList();

        context.ActionArguments["userData"] = new UserData(
            Convert.ToInt64(userClaims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
            userClaims.First(c => c.Type == ClaimTypes.Name).Value,
            userClaims.First(c => c.Type == ClaimTypes.Email).Value);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}