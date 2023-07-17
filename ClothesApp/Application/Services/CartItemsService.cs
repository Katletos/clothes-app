using Application.Dtos.CartItem;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CartItemsService : ICartItemsService
{
    private readonly ICartItemsRepository _cartItemsRepository;

    private readonly IUserRepository _userRepository;

    private readonly IProductsRepository _productsRepository;

    private readonly IMapper _mapper;

    public CartItemsService(ICartItemsRepository cartItemsRepository, IMapper mapper, IUserRepository userRepository,
        IProductsRepository productsRepository)
    {
        _cartItemsRepository = cartItemsRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _productsRepository = productsRepository;
    }

    public async Task<IList<CartItemDto>> GetUserCartItems(long userId)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.UserNotFound);
        }

        var cartItems = await _cartItemsRepository.GetByUserId(userId);
        var cartItemDtos = _mapper.Map<IList<CartItemDto>>(cartItems);
        return cartItemDtos;
    }

    public async Task<CartItemDto> Add(CartItemDto cartItemDto)
    {
        var exist = await _userRepository.DoesExist(cartItemDto.UserId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(cartItemDto.ProductId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(cartItemDto.UserId, cartItemDto.ProductId);

        if (exist)
        {
            await DoesEnoughQuantity(cartItemDto);
            await _cartItemsRepository.UpdateQuantity(cartItemDto.ProductId, +cartItemDto.Quantity);
        }
        else
        {
            await DoesEnoughQuantity(cartItemDto);
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartItemsRepository.Insert(cartItem);
        }

        await _productsRepository.UpdateQuantity(cartItemDto.ProductId, -cartItemDto.Quantity);

        var updatedCartItem = await _cartItemsRepository.GetItem(cartItemDto.ProductId, cartItemDto.UserId);
        var updatedCartItemDto = _mapper.Map<CartItemDto>(updatedCartItem);
        return updatedCartItemDto;
    }

    public async Task<CartItemDto> Remove(CartItemDto cartItemDto)
    {
        var exist = await _userRepository.DoesExist(cartItemDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(cartItemDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(cartItemDto.UserId, cartItemDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.CartItemNotFound);
        }

        var cartItem = await _cartItemsRepository.GetItem(cartItemDto.ProductId, cartItemDto.UserId);
        var enough = cartItem.Quantity >= cartItemDto.Quantity;

        if (!enough)
        {
            throw new BusinessRuleException(Messages.NotEnoughQuantity);
        }

        cartItem.Quantity -= cartItemDto.Quantity;
        await _cartItemsRepository.Update(cartItem);
        //    await _cartItemsRepository.UpdateQuantity(cartItemDto.ProductId, -cartItemDto.Quantity);
        await _productsRepository.UpdateQuantity(cartItemDto.ProductId, +cartItemDto.Quantity);

        if (cartItem.Quantity == 0)
        {
            await _cartItemsRepository.Delete(cartItem);
        }

        var updatedCartItemDto = _mapper.Map<CartItemDto>(cartItem);
        return updatedCartItemDto;
    }

    public async Task<CartItemDto> Drop(CartItemDto cartItemDto)
    {
        var exist = await _userRepository.DoesExist(cartItemDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(cartItemDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(cartItemDto.UserId, cartItemDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.CartItemNotFound);
        }

        var cartItem = await _cartItemsRepository.GetItem(cartItemDto.ProductId, cartItemDto.UserId);
        await _productsRepository.UpdateQuantity(cartItemDto.ProductId, +cartItem.Quantity);
        await _cartItemsRepository.Delete(cartItem);

        return _mapper.Map<CartItemDto>(cartItem);
    }

    public async Task<long> GetReservedProductQuantity(long productId)
    {
        return await _cartItemsRepository.GetReservedProductQuantity(productId);
    }

    public async Task<long> GetAvailableProductQuantity(long productId)
    {
        return await _cartItemsRepository.GetAvailableProductQuantity(productId);
    }

    private async Task<bool> DoesEnoughQuantity(CartItemDto cartItemDto)
    {
        var product = await _productsRepository.GetById(cartItemDto.ProductId);
        var enough = product.Quantity >= cartItemDto.Quantity;

        if (!enough)
        {
            throw new BusinessRuleException(Messages.NotEnoughQuantity);
        }

        return true;
    }
}