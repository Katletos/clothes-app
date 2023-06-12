using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Media;

public class MediaInputDto
{
    public long ProductId { get; set; }

    public IFormFile File { get; set; }

}