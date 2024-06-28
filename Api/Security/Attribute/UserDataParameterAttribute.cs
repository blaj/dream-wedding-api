using System.Security.Claims;
using DreamWeddingApi.Api.Security.DTO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DreamWeddingApi.Api.Security.Attribute;

public class UserDataParameterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        
        var userClaims = context.HttpContext.User.Claims.ToList();

        context.ActionArguments["userData"] = new UserData(
            Convert.ToInt64(userClaims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
            userClaims.First(c => c.Type == ClaimTypes.Name).Value,
            userClaims.First(c => c.Type == ClaimTypes.Email).Value);
    }
}