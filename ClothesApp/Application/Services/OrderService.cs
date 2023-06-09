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

    private readonly IAddressService _addressService;
    
    private readonly IUserRepository _userRepository;

    private readonly IOrderItemsRepository _orderItemsRepository;

    private readonly IOrderTransactionsRepository _transactionsRepository;

    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper,
        IOrderTransactionsRepository transactionsRepository, IUserRepository userRepository,
        IAddressService addressService, IOrderItemsRepository orderItemsRepository,
        IProductsRepository productsRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _transactionsRepository = transactionsRepository;
        _userRepository = userRepository;
        _addressService = addressService;
        _orderItemsRepository = orderItemsRepository;
        _productsRepository = productsRepository;
    }

    public async Task<OrderDto> SubmitOrder(long id)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var order = await _orderRepository.GetById(id);

        if (order.OrderStatus != OrderStatusType.OnHold)
        {
            throw new BusinessRuleException();
        }
        
        var orderDto = _mapper.Map<Order, OrderDto>(order, opt =>
            opt.BeforeMap((src, _) => src.OrderStatus = OrderStatusType.InReview));
        await _orderRepository.Update(order);
        
        return orderDto;
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
        var exist = await _userRepository.DoesExist(orderInputDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var belongs = await _addressService.DoesAddressBelongUser(orderInputDto.AddressId, orderInputDto.UserId);

        if (!belongs)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        var order = _mapper.Map<OrderInputDto, Order>(orderInputDto, opt =>
            opt.AfterMap((_, dest) =>
            {
                dest.OrderStatus = OrderStatusType.OnHold;
                dest.CreatedAt = DateTime.Now;
            }));
        await _orderRepository.Insert(order);
     
        var orderWithId = await _orderRepository.GetLastUserOrder(o => o.CreatedAt == order.CreatedAt
        && o.UserId == order.UserId);
        
        decimal sum = 0;
        foreach (var item in orderInputDto.OrderItems)
        {
            await ReserveProduct(item);
            sum += await CalcProductPrice(item);
        }

        orderWithId.Price = sum;
        await _orderRepository.Update(orderWithId);


        var orderItemDtos = _mapper.Map<IList<OrderItemDto>>(orderInputDto.OrderItems);
        IList<OrderItem> orderItems = new List<OrderItem>();
        foreach (var item in orderItemDtos)
        {
            var product = await _productsRepository.GetById(item.ProductId);
            var orderItem = _mapper.Map<OrderItemDto, OrderItem>(item, opt =>
                opt.AfterMap((_, dest) =>
                {
                    dest.OrderId = orderWithId.Id;
                    dest.Price = product.Price;
                }));
            
            orderItems.Add(orderItem);
        }

        await _orderItemsRepository.InsertRange(orderItems);
        
        var orderDto = _mapper.Map<OrderDto>(orderWithId);

        return orderDto;
    }

    public async Task<decimal> CalcOrderPrice(long id)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException();
        }

        var orderItems = await _orderItemsRepository.GetByOrderId(id);

        decimal sum = 0;
        foreach (var item in orderItems)
        {
            sum += item.Price * item.Quantity;
        }

        return sum;
    }

    public async Task<decimal> CalcProductPrice(OrderItemInputDto orderItemInputDto)
    {
        var exist = await _productsRepository.DoesExist(orderItemInputDto.ProductId);

        if (!exist)
        {
            throw new BusinessRuleException();
        }

        var product = await _productsRepository.GetById(orderItemInputDto.ProductId);
        decimal sum = product.Price * orderItemInputDto.Quantity;
            
        return sum;
    }

    public async Task ReserveProduct(OrderItemInputDto orderItemInputDto)
    {
        var exist = await _productsRepository.DoesExist(orderItemInputDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        var product = await _productsRepository.GetById(orderItemInputDto.ProductId);
        
        var enough = product.Quantity - orderItemInputDto.Quantity > 0;

        if (!enough)
        {
            throw new BusinessRuleException("not enough");
        }

        product.Quantity -= orderItemInputDto.Quantity;
        
        await _productsRepository.Update(product);
    }

    public async Task<OrderItemDto> AddItem(long orderId, OrderItemInputDto orderItemInputDto)
    {
        var exist = await _productsRepository.DoesExist(orderItemInputDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _orderRepository.DoesExist(orderId);
        
        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var order = await _orderRepository.GetById(orderId);

        if (order.OrderStatus != OrderStatusType.OnHold)
        {
            throw new BusinessRuleException();
        }

        exist = await _orderItemsRepository.DoesExistByGetByProductId(orderId,orderItemInputDto.ProductId);

        if (exist)
        {
            throw new BusinessRuleException();
        }

        var product = await _productsRepository.GetById(orderItemInputDto.ProductId);
        var productPrice = product.Price;
        
        var orderItem = _mapper.Map<OrderItemInputDto, OrderItem>(orderItemInputDto, opt => 
            opt.AfterMap((_, dest) =>
            {
                dest.OrderId = orderId;
                dest.Price = productPrice;
            }));
        await _orderItemsRepository.Insert(orderItem);
        
        await ReserveProduct(orderItemInputDto);
        order.Price = await CalcOrderPrice(order.Id);
        await _orderRepository.Update(order);
        
        var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);

        return orderItemDto;
    }

    public async Task<OrderDto> UpdateStatus(long id)
    {
        var exist = await _orderRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var order = await _orderRepository.GetById(id);

        if (order.OrderStatus == OrderStatusType.Completed)
        {
            throw new BusinessRuleException();
        }
        
        order.OrderStatus += 1;
        await _orderRepository.Update(order);

        var orderTransaction = new OrderTransaction()
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus,
            UpdatedAt = DateTime.Now,
        };
        await _transactionsRepository.Add(orderTransaction);

        var orderDto = _mapper.Map<OrderDto>(order);
        return orderDto;
    }

    public Task<OrderDto> DeleteById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id)
    {
        var exist = await _orderRepository.DoesExist(id);
        
        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
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
            throw new NotFoundException(Messages.NotFound);
        }

        var orderItems = await _orderItemsRepository.GetByOrderId(orderId);
        var orderItemsDto = _mapper.Map<IList<OrderItemDto>>(orderItems);

        return orderItemsDto;
    }
}