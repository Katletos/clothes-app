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
    private readonly IOrderRepository _orderRepository;

    private readonly IOrderTransactionsRepository _transactionsRepository;

    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, IOrderTransactionsRepository transactionsRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _transactionsRepository = transactionsRepository;
    }

    public async Task<IList<OrderDto>> GetAll()
    {
        var orders = await _orderRepository.GetAll();
        var ordersDto = _mapper.Map<IList<OrderDto>>(orders);
        return ordersDto;
    }

    public async Task<OrderDto> GetById(long id)
    {
        var order = await _orderRepository.GetById(id);

        if (order is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }

    public async Task<OrderDto> Add(OrderInputDto orderInputDto)
    {
        var order = _mapper.Map<Order>(orderInputDto);
        await _orderRepository.Insert(order);
        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }

    public async Task<OrderDto> UpdateStatus(long id, OrderStatusType statusType)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var order = await _orderRepository.GetById(id);
        var orderDto = _mapper.Map<Order, OrderDto>(order, opt =>
            opt.BeforeMap((src, _) => src.OrderStatus = statusType));

        await _orderRepository.Update(order);

        var orderTransaction = _mapper.Map<OrderTransaction>(order);
        await _transactionsRepository.Add(orderTransaction);

        return orderDto;
    }

    public Task<OrderDto> DeleteById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<OrderItemDto>> GetOrderItemsByOrderId(long orderId)
    {
        throw new NotImplementedException();
    }
}