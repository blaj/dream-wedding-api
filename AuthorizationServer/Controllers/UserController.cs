using System.Security.Claims;
using DreamWeddingApi.AuthorizationServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService = DreamWeddingApi.AuthorizationServer.Service.AuthenticationService;

namespace DreamWeddingApi.AuthorizationServer.Controllers;

public class UserController(AuthenticationService authenticationService) : Controller
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

        var principal =
            authenticationService.GetPrincipal(userLoginModel.Username, userLoginModel.Password);

        if (principal is null)
        {
            return View(userLoginModel);
        }
        
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