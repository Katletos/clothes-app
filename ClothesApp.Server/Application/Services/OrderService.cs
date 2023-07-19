using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IProductsRepository _productsRepository;

    private readonly IOrderRepository _orderRepository;

    private readonly IAddressRepository _addressRepository;

    private readonly IUserRepository _userRepository;

    private readonly IOrderItemsRepository _orderItemsRepository;

    private readonly IOrderTransactionsRepository _transactionsRepository;

    private readonly ICartItemsService _cartItemsService;

    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper,
        IOrderTransactionsRepository transactionsRepository, IUserRepository userRepository,
        IAddressRepository addressRepository, IOrderItemsRepository orderItemsRepository,
        IProductsRepository productsRepository, ICartItemsService cartItemsService)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _transactionsRepository = transactionsRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _orderItemsRepository = orderItemsRepository;
        _productsRepository = productsRepository;
        _cartItemsService = cartItemsService;
    }

    public async Task<IList<OrderDto>> GetAll()
    {
        var orders = await _orderRepository.GetAll();
        var ordersDto = _mapper.Map<IList<OrderDto>>(orders);
        return ordersDto;
    }

    public async Task<OrderDto> GetById(long id)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.OrderNotFound);
        }

        var order = await _orderRepository.GetById(id);
        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }

    public async Task<OrderDto> Add(OrderInputDto orderInputDto)
    {
        var exist = await _userRepository.DoesExist(orderInputDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        var belongs = await _addressRepository.DoesAddressBelongToUser(orderInputDto.AddressId, orderInputDto.UserId);

        if (!belongs)
        {
            throw new BusinessRuleException(Messages.AddressUserConstraint);
        }

        var productIds = orderInputDto.OrderItems.Select(oi => oi.ProductId).ToList();
        exist = await _productsRepository.DoesExistRange(productIds);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        var order = new Order()
        {
            OrdersItems = new List<OrderItem>(),
        };
        var orderItem = new OrderItem();
        var cartItems = await _cartItemsService.GetUserCartItems(orderInputDto.UserId);

        var doesSameItems = cartItems.Select(ci => ci.ProductId).ToHashSet().SetEquals(productIds.ToHashSet());
        if (!doesSameItems)
        {
            throw new NotFoundException(Messages.CartItemNotFound);
        }

        foreach (var cartItem in cartItems)
        {
            var lessThanHalf = cartItem.Quantity > cartItem.Product.Quantity;

            if (lessThanHalf)
            {
                throw new BusinessRuleException(Messages.MoreThanInStockQuantity);
            }

            orderItem.ProductId = cartItem.ProductId;
            orderItem.Quantity = cartItem.Quantity;
            orderItem.Price = cartItem.Product.Price;
            orderItem.OrderId = order.Id;

            order.OrdersItems.Add(orderItem);

            await _cartItemsService.Delete(cartItem.ProductId, orderInputDto.UserId);
        }

        order.Price = CalcOrderPrice(order.OrdersItems);
        order.OrderStatus = OrderStatusType.InReview;
        order.AddressId = orderInputDto.AddressId;
        order.UserId = orderInputDto.UserId;
        order.CreatedAt = DateTime.Now;
        await _orderRepository.Insert(order);

        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }

    private decimal CalcOrderPrice(ICollection<OrderItem> orderItems)
    {
        decimal sum = 0;
        foreach (var item in orderItems)
        {
            sum += item.Price * item.Quantity;
        }

        return sum;
    }

    public async Task<OrderDto> UpdateStatus(long orderId, OrderStatusType newOrderStatus)
    {
        var exist = await _orderRepository.DoesExist(orderId);

        if (!exist)
        {
            throw new NotFoundException(Messages.OrderNotFound);
        }

        var order = await _orderRepository.GetById(orderId);

        if (order.OrderStatus == OrderStatusType.Completed || order.OrderStatus == OrderStatusType.Cancelled)
        {
            throw new BusinessRuleException(Messages.OrderUpdateConstraint);
        }

        order.OrderStatus = OrderStateMachine(order.OrderStatus, newOrderStatus);
        await _orderRepository.Update(order);

        var orderTransaction = new OrderTransaction()
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus,
            UpdatedAt = DateTime.Now,
        };
        await _transactionsRepository.Insert(orderTransaction);

        var orderDto = _mapper.Map<OrderDto>(order);
        return orderDto;
    }

    private OrderStatusType OrderStateMachine(OrderStatusType currentState, OrderStatusType newState)
    {
        var result = (currentState, newState) switch
        {
            (OrderStatusType.InReview, OrderStatusType.InDelivery) => OrderStatusType.InDelivery,
            (OrderStatusType.InReview, OrderStatusType.Cancelled) => OrderStatusType.Cancelled,
            (OrderStatusType.InDelivery, OrderStatusType.Completed) => OrderStatusType.Completed,
            (OrderStatusType.InDelivery, OrderStatusType.Cancelled) => OrderStatusType.Cancelled,
            _ => throw new BusinessRuleException(Messages.OrderTransitionConstraint),
        };

        return result;
    }

    public async Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.OrderNotFound);
        }

        var orderTransactions = await _transactionsRepository.GetByOrderId(id);

        var orderTransactionsDto = _mapper.Map<IList<OrderTransactionsDto>>(orderTransactions);

        return orderTransactionsDto;
    }

    public async Task<IList<OrderItemDto>> GetOrderItemsByOrderId(long orderId)
    {
        var exist = await _orderRepository.DoesExist(orderId);

        if (!exist)
        {
            throw new NotFoundException(Messages.OrderNotFound);
        }

        var orderItems = await _orderItemsRepository.GetByOrderId(orderId);
        var orderItemsDto = _mapper.Map<IList<OrderItemDto>>(orderItems);

        return orderItemsDto;
    }

    public async Task<IList<OrderDto>> GetOrdersByStatus(OrderStatusType status)
    {
        var orders = await _orderRepository.FindByCondition(o => o.OrderStatus == status);
        var orderDtos = _mapper.Map<IList<OrderDto>>(orders);
        return orderDtos;
    }
}