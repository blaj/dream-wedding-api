using DreamWeddingApi.Wedding.DTO;
using DreamWeddingApi.Wedding.Service;
using Microsoft.AspNetCore.Mvc;

namespace DreamWeddingApi.Wedding.Controller;

[ApiController]
[Route("/wedding")]
public class WeddingController : ControllerBase
{

    private readonly WeddingService _weddingService;

    public WeddingController(WeddingService weddingService)
    {
        _weddingService = weddingService;
    }

    [HttpGet]
    public IEnumerable<Entity.Wedding> list()
    {
        return _weddingService.getList();
    }
}