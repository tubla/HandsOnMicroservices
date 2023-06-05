using AutoMapper;
using Order.Application.Features.Orders.Command.CheckoutOrder;
using Order.Application.Features.Orders.Queries.GetOrderList;
using Order.Domain.Entities;

namespace Order.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderEntity, OrdersVm>().ReverseMap();  
            CreateMap<OrderEntity,CheckoutOrderCommand>().ReverseMap();
        }
    }
}
