using Application.Dtos.Sections;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;

    private readonly ISectionCategoryRepository _sectionCategoryRepository;

    private readonly IMapper _mapper;

    public SectionService(ISectionRepository sectionRepository, IMapper mapper,
        ISectionCategoryRepository sectionCategoryRepository)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
        _sectionCategoryRepository = sectionCategoryRepository;
    }

    public async Task<IList<SectionDto>> GetAll()
    {
        var products = await _sectionRepository.GetAll();
        var productsDto = _mapper.Map<IList<SectionDto>>(products);
        return productsDto;
    }

    public async Task<SectionDto> GetById(long id)
    {
        var product = await _sectionRepository.GetById(id);

        if (product is null)
        {
            throw new NotFoundException(Messages.SectionNotFound);
        }

        var productDto = _mapper.Map<SectionDto>(product);
        return productDto;
    }

    public async Task<SectionDto> Add(SectionInputDto sectionInputDto)
    {
        var exists = await _sectionRepository.DoesExist(sectionInputDto.Name);

        if (exists)
        {
            throw new BusinessRuleException(Messages.SectionUniqueConstraint);
        }

        var section = _mapper.Map<Section>(sectionInputDto);
        await _sectionRepository.Insert(section);
        var sectionDto = _mapper.Map<SectionDto>(section);

        return sectionDto;
    }

    public async Task<SectionDto> Update(long id, SectionInputDto sectionInputDto)
    {
        var section = _mapper.Map<SectionInputDto, Section>(sectionInputDto, opt =>
            opt.AfterMap((_, dest) => dest.Id = id));

        var exist = await _sectionRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.SectionNotFound);
        }

        var sameName = await _sectionRepository.AreSameName(id, sectionInputDto.Name);

        if (sameName)
        {
            throw new BusinessRuleException(Messages.SectionUniqueConstraint);
        }

        await _sectionRepository.Update(section);
        var sectionDto = _mapper.Map<SectionDto>(section);

        return sectionDto;
    }

    public async Task<SectionDto> DeleteById(long id)
    {
        var exist = await _sectionRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.SectionNotFound);
        }

        var relates = await _sectionCategoryRepository.DoesSectionRelateCategory(id);

        if (relates)
        {
            throw new BusinessRuleException(Messages.SectionDeleteConstraint);
        }

        var section = await _sectionRepository.GetById(id);
        await _sectionRepository.Delete(section);
        var sectionDto = _mapper.Map<SectionDto>(section);

        return sectionDto;
    }
}