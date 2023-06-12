using Application.Dtos.Media;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IMediaService
{
    Task<IList<MediaDto>> GetByProductId(long id);

    Task<MediaDto> UploadFile(MediaInputDto mediaInputDto);
}