using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Order.Application.Features.Orders.Command.CheckoutOrder;
using System.Diagnostics;

namespace Order.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            //Debugger.Launch();
            //Debugger.Break();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var orderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(orderCommand);
            _logger.LogInformation($"BasketCheckout event is successfully consumed. Created order id {result}");
        }
    }
}
