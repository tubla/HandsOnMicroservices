using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Domain.Entities;
using Order.Infrastructure.Persistance;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<OrderEntity>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
                
        }
        public async Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName)
        {
            var orders = await _orderContext.Orders.ToListAsync();
            var orderList = await _orderContext.Orders.Where(o => o.UserName == userName).ToListAsync();
            return orderList;
        }
    }
}
