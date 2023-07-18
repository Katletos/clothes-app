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

    public async Task<CartItemDto> Add(long productId, long userId)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(productId);

        if (!exist)
        {
            throw new BusinessRuleException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(userId, productId);

        if (exist)
        {
            throw new BusinessRuleException(Messages.CartItemAlreadyExist);
        }

        var product = await _productsRepository.GetById(productId);
        var enough = product.Quantity > 0;

        if (!enough)
        {
            throw new BusinessRuleException(Messages.NotEnoughQuantity);
        }

        product.Quantity -= 1;
        await _productsRepository.Update(product);
        // await _productsRepository.UpdateQuantity(productId, product.Quantity - 1);
        CartItem cartItem = new()
        {
            Quantity = 1,
            ProductId = productId,
            UserId = userId,
        };
        await _cartItemsRepository.Insert(cartItem);

        var cartItemDto = _mapper.Map<CartItemDto>(cartItem);
        return cartItemDto;
    }

    public async Task<CartItemDto> Update(long productId, long userId, long newQuantity)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(productId);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(userId, productId);

        if (!exist)
        {
            throw new NotFoundException(Messages.CartItemNotFound);
        }

        var product = await _productsRepository.GetById(productId);
        var cartItem = await _cartItemsRepository.GetItem(productId, userId);

        var enough = product.Quantity + cartItem.Quantity >= newQuantity;

        if (!enough)
        {
            throw new BusinessRuleException(Messages.NotEnoughQuantity);
        }

        var delta = cartItem.Quantity - newQuantity;
        product.Quantity += delta;
        cartItem.Quantity = newQuantity;
        await _productsRepository.Update(product);
        await _cartItemsRepository.Update(cartItem);

        if (cartItem.Quantity == 0)
        {
            await _cartItemsRepository.Delete(cartItem);
        }

        return _mapper.Map<CartItemDto>(cartItem);
    }

    public async Task<CartItemDto> Delete(long productId, long userId)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        exist = await _productsRepository.DoesExist(productId);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        exist = await _cartItemsRepository.DoesExist(userId, productId);

        if (!exist)
        {
            throw new NotFoundException(Messages.CartItemNotFound);
        }

        var cartItem = await _cartItemsRepository.GetItem(productId, userId);
        var product = await _productsRepository.GetById(productId);
        product.Quantity += cartItem.Quantity;
        await _productsRepository.Update(product);
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
}