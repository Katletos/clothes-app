using AutoMapper;
using ClothesApp.Data;
using ClothesApp.Data.Dtos;
using ClothesApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothesApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly ClothesAppContext _dbContext;

    private readonly IMapper _mapper;
    
    public BrandsController(ClothesAppContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
     public async Task<ActionResult<IReadOnlyCollection<BrandDto>>> GetAllBrands()
     { 
         var brands = await _dbContext.Brands.ToListAsync();
         
         var brandDtos = _mapper.Map<IReadOnlyCollection<BrandDto>>(brands);
         
         return Ok(brandDtos);
     }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetBrandById(long id)
    {
        if (id <= 0)
        {
            return BadRequest($"Brand Id = '{id}' can't be less or equal zero");
        }
        
        var brand = await _dbContext.Brands.FindAsync(id);

        if (brand is null)
        {
            return NotFound($"Brand with Id = '{id}' not found");
        }
        
        var brandDto = _mapper.Map<BrandDto>(brand);
        return Ok(brandDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrandById(long id)
    {
        if (id <= 0)
        {
            return BadRequest($"Brand Id = '{id}' can't be less or equal zero");
        }
        
        var brand = await _dbContext.Brands.FindAsync(id);

        if (brand is null)
        {
            return NotFound($"Brand with Id = '{id}' not found");
        }

        var productExists = await _dbContext.Products.AnyAsync(p => p.BrandId == id);

        if (productExists)
        {
            return Conflict($"Brand with Id = '{id}' has one or more product");
        }
        
        _dbContext.Brands.Remove(brand);
        await _dbContext.SaveChangesAsync();

        var brandDto = _mapper.Map<BrandDto>(brand);
        
        return Ok(brandDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddBrand(CreateBrandDto createBrandDto)
    {
        var brandExists = await _dbContext.Brands.AnyAsync(b => b.Name == createBrandDto.Name);
        
        if (brandExists)
        {
            return Conflict($"Brand Name = '{createBrandDto.Name}' already exists");
        }
        
        var brand = _mapper.Map<Brand>(createBrandDto);
        await _dbContext.Brands.AddAsync(brand);
        await _dbContext.SaveChangesAsync();

        var brandDto = _mapper.Map<BrandDto>(brand);
        return Ok(brandDto);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBrand(long id ,BrandDto brandDto)
    {
        if (id != brandDto.Id)
        {
            return BadRequest("Brand Id mismatch");
        }
        
        var brand = await _dbContext.Brands.FindAsync(id);

        if (brand is null)
        {
            return NotFound($"Brand with Id = {id} not found");
        }
       
        brand.Name = brandDto.Name;
        await _dbContext.SaveChangesAsync();
       
        var updatedBrandDto = _mapper.Map<BrandDto>(brand);
        
        return Ok(updatedBrandDto);
    }
}