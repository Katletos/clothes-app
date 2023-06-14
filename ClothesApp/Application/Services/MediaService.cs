using Application.Dtos.Media;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;

    private readonly IProductsRepository _productsRepository;

    private readonly IMapper _mapper;

    public MediaService(IMediaRepository mediaRepository, IMapper mapper, IProductsRepository productsRepository)
    {
        _mediaRepository = mediaRepository;
        _mapper = mapper;
        _productsRepository = productsRepository;
    }

    public async Task<long[]> ImageIdsGetByProductId(long id)
    {
        var exist = await _productsRepository.DoesExist(id);

        if (!exist) throw new NotFoundException(Messages.ProductNotFound);

        return await _mediaRepository.GetImageIdsByProductId(id);
    }

    public async Task<MediaDto> UploadFile(MediaInputDto mediaInputDto)
    {
        var exist = await _productsRepository.DoesExist(mediaInputDto.ProductId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.ProductNotFound);
        }

        var file = mediaInputDto.File;
        var media = new Media();
        if (file.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            if (memoryStream.Length < 50_000_000)
            {
                media.Bytes = memoryStream.ToArray();
                media.FileName = file.FileName;
                media.FileType = GetMimeType(Path.GetExtension(file.FileName));
                media.ProductId = mediaInputDto.ProductId;
            }
            else
            {
                throw new BusinessRuleException(Messages.FileUploadConstraint);
            }
        }
        else
        {
            throw new BusinessRuleException(Messages.EmptyFile);
        }

        await _mediaRepository.Insert(media);
        var mediaDto = _mapper.Map<MediaDto>(media);
        return mediaDto;
    }

    private string GetMimeType(string extension)
    {
        var mappings = new Dictionary<string, string>()
        {
            { ".jpe", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" }
        };

        return mappings.TryGetValue(extension, out var mime) ? mime : "application/octet-stream";
    }

    public async Task<Media> GetMedia(long id)
    {
        var exist = await _productsRepository.DoesExist(id);

        if (!exist) throw new BusinessRuleException(Messages.ProductNotFound);

        return await _mediaRepository.GetById(id);
    }
}