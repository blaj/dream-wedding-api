using DreamWeddingApi.Api.Security.Attribute;
using DreamWeddingApi.Api.Security.DTO;
using DreamWeddingApi.Api.Wedding.DTO;
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

    [HttpGet("/wedding")]
    public IEnumerable<WeddingListItemDto> List()
    {
        return _weddingService.GetList();
    }

    [HttpGet("/wedding/{id}")]
    [UserDataParameter]
    public ActionResult<WeddingDetailsDto> Get(long id, UserData userData)
    {
        var weddingDetailsDto = _weddingService.GetOne(id);

        if (weddingDetailsDto is null)
        {
            return NotFound();
        }

        return Ok(weddingDetailsDto);
    }

    [HttpPost]
    public ActionResult<long> Create(WeddingCreateRequest weddingCreateRequest)
    {
        var createdEntity = _weddingService.Create(weddingCreateRequest);

        return Created(
            Url.Action("Get", "Wedding", new { id = createdEntity.Id }),
            createdEntity.Id);
    }

    [HttpPut("/wedding/{id}")]
    public ActionResult Update(long id, WeddingUpdateRequest weddingUpdateRequest)
    {
        _weddingService.Update(id, weddingUpdateRequest);

        return NoContent();
    }

    [HttpDelete("/wedding/{id}")]
    public ActionResult Delete(long id)
    {
        _weddingService.Delete(id);

        return Accepted();
    }
}