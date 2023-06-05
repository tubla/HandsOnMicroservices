using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Persistence;
using Order.Application.Exceptions;
using Order.Domain.Entities;

namespace Order.Application.Features.Orders.Command.DeleteOrder
{
    public class DeleteOrderHandlerClass : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderHandlerClass> _logger;

        public DeleteOrderHandlerClass(IOrderRepository orderRepository, IMapper mapper,ILogger<DeleteOrderHandlerClass> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if(orderEntity == null)
            {
                throw new NotFoundException(nameof(OrderEntity), request.Id);
            }
            _mapper.Map(request,orderEntity,typeof(DeleteOrderCommand),typeof(OrderEntity));
            await _orderRepository.DeleteAsync(orderEntity);
            _logger.LogInformation($"Order with order id {request.Id} is successfully deleted");
            return Unit.Value;

        }
    }
}
