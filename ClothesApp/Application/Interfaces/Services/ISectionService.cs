using Application.Dtos.Sections;

namespace Application.Interfaces.Services;

public interface ISectionService
{
    Task<IList<SectionDto>> GetAll();

    Task<SectionDto> GetById(long id);

    Task<SectionDto> Add(SectionDto sectionDto);

    Task<SectionDto> Update(long id, SectionInputDto sectionInputDto);

    Task<SectionDto> DeleteBuId(long id);
}