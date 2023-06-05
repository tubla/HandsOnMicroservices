using Order.Domain.Entities;

namespace Order.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<OrderEntity>
    {
        Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName);
    }
}
