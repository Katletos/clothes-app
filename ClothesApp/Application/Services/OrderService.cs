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

    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper,
        IOrderTransactionsRepository transactionsRepository, IUserRepository userRepository,
        IAddressRepository addressRepository, IOrderItemsRepository orderItemsRepository,
        IProductsRepository productsRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _transactionsRepository = transactionsRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _orderItemsRepository = orderItemsRepository;
        _productsRepository = productsRepository;
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
        
        var orderItems =  await ReserveOrderProducts(orderInputDto);
        var order = _mapper.Map<OrderInputDto, Order>(orderInputDto);

        order.Price = CalcOrderPrice(orderItems);
        order.OrdersItems = orderItems;
        order.OrderStatus = OrderStatusType.InReview;
        await _orderRepository.Insert(order);
        
        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }

    private decimal CalcOrderPrice(IList<OrderItem> orderItems)
    {
        decimal sum = 0;
        foreach (var item in orderItems)
        {
            sum += item.Price * item.Quantity;
        }
        return sum;
    }

    public async Task<IList<OrderItem>> ReserveOrderProducts(OrderInputDto orderInputDto)
    {
        var orderItems = _mapper.Map<IList<OrderItem>>(orderInputDto.OrderItems);

        var productIds = new List<long>();
        foreach (var item in orderItems)
        {
            productIds.Add(item.ProductId);
        }
        
        var exist = await _productsRepository.DoesExistRange(productIds);
        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        var products = await _productsRepository.GetRange(productIds);
        for (int i = 0; i < products.Count; i++)
        {
            var enough = products[i].Quantity - orderItems[i].Quantity >= 0;

            if (!enough)
            {
                throw new BusinessRuleException($"Product with id {products[i].Id} don't have enough quantity on stock");
            }

            products[i].Quantity -= orderItems[i].Quantity;
            
            orderItems[i].Price = products[i].Price;
            orderItems[i].ProductId = products[i].Id;
        }

        await _productsRepository.UpdateRange(products);

        return orderItems;
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