using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Infrastructure;
using Order.Application.Contracts.Persistence;
using Order.Application.Models;
using Order.Domain.Entities;

namespace Order.Application.Features.Orders.Command.CheckoutOrder
{
    public class CheckoutOrderHandlerClass : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderHandlerClass> _logger;

        public CheckoutOrderHandlerClass(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderHandlerClass> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<OrderEntity>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"New order with order id {newOrder.Id} is successfully created");
            await SendMail(newOrder);
            return newOrder.Id;

        }

        private async Task SendMail(OrderEntity newOrder)
        {
            var email = new Email() { To = "rahulroy.profile@gmail.com", Subject = $"Order Created #{newOrder.Id}", Body = "New Order Created" };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Could not send mail due to an error in the mail service. Error - {ex.InnerException}");   
            }
        }
    }
}
