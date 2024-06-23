using DreamWeddingApi.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace DreamWeddingApi.User.Controller;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
}