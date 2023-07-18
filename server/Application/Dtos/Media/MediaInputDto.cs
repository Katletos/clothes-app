using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Media;

public struct MediaInputDto
{
    public long ProductId { get; set; }

    public IFormFile File { get; set; }

}