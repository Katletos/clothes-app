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

    public async Task<IList<MediaDto>> GetByProductId(long id)
    {
        var media = await _mediaRepository.FindByCondition(m => m.ProductId == id);
        var mediaDto = _mapper.Map<IList<MediaDto>>(media);
        return mediaDto;
    }

    public async Task<MediaDto> UploadFile(MediaInputDto mediaInputDto)
    {
        var exist = await _productsRepository.DoesExist(mediaInputDto.ProductId);

        if (!exist)
        {
            throw new BusinessRuleException();
        }

        var guid = Guid.NewGuid().ToString();
        var fileName = guid + "-" + mediaInputDto.File.FileName;
        var filePath = Path.Combine(@"Images", fileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await mediaInputDto.File.CopyToAsync(stream);
        }

        Media media = new Media()
        {
            FileName = fileName,
            ProductId = mediaInputDto.ProductId,
            Url = filePath,
            FileType = mediaInputDto.File.ContentType,
        };

        await _mediaRepository.Insert(media);
        var mediaDto = _mapper.Map<MediaDto>(media);
        return mediaDto;
    }
}