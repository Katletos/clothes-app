using Application.Dtos.Media;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/medias")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MediaDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetListOfImageIds(long id)
    {
        var mediaDtos = await _mediaService.GetImageIdsByProductId(id);

        return Ok(mediaDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponseMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DownloadImage(long id)
    {
        var media = await _mediaService.GetMedia(id);

        return File(media.Bytes, media.FileType, media.FileName);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MediaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddMedia([FromForm] MediaInputDto mediaInputDto)
    {
        var mediaDto = await _mediaService.UploadFile(mediaInputDto);

        return Ok(mediaDto);
    }
}