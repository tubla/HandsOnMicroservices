using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Infrastructure;
using Order.Application.Contracts.Persistence;
using Order.Application.Models;
using Order.Domain.Entities;

namespace Order.Application.Features.Orders.Command.UpdateOrder
{
    public class UpdateOrderHandlerClass : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderHandlerClass> _logger;

        public UpdateOrderHandlerClass(IOrderRepository orderRepository, IMapper mapper,ILogger<UpdateOrderHandlerClass> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if(orderEntity == null)
            {
                _logger.LogError($"Order with id{request.Id} does not exist in database");               
            }
            _mapper.Map(request,orderEntity,typeof(UpdateOrderCommand),typeof(OrderEntity));
            await _orderRepository.UpdateAsync(orderEntity);
            _logger.LogInformation($"Order with order id {request.Id} is successfully updated");
            return Unit.Value;

        }
    }
}
