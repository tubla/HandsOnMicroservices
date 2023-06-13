using AutoMapper;
using EventBus.Messages.Events;
using Order.Application.Features.Orders.Command.CheckoutOrder;

namespace Order.API.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CheckoutOrderCommand,BasketCheckoutEvent>().ReverseMap();
        }
    }
}
