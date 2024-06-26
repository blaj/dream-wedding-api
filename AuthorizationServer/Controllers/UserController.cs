using System.Security.Claims;
using DreamWeddingApi.AuthorizationServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamWeddingApi.AuthorizationServer.Controllers;

public class UserController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginModel userLoginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(userLoginModel);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userLoginModel.Username),
        };

        var principal =
            new ClaimsPrincipal(
                new List<ClaimsIdentity>
                {
                    new(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                });

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        if (!string.IsNullOrEmpty(userLoginModel.ReturnUrl))
        {
            return Redirect(userLoginModel.ReturnUrl);
        }

        return View(userLoginModel);
    }
}