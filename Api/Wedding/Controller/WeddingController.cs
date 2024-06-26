using DreamWeddingApi.Api.Wedding.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamWeddingApi.Api.Wedding.Controller;

[ApiController]
[Authorize]
[Route("/wedding")]
public class WeddingController : ControllerBase
{
    private readonly WeddingService _weddingService;

    public WeddingController(WeddingService weddingService)
    {
        _weddingService = weddingService;
    }

    [HttpGet]
    public IEnumerable<Api.Wedding.Entity.Wedding> list()
    {
        return _weddingService.GetList();
    }
}