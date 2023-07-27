using Application.Dtos.Media;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IMediaService
{
    Task<long[]> GetImageIdsByProductId(long id);

    Task<MediaDto> UploadFile(MediaInputDto mediaInputDto);

    Task<Media> GetMedia(long id);
}