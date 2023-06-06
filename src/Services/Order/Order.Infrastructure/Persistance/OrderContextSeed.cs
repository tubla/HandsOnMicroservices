using Microsoft.Extensions.Logging;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistance
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext,ILogger<OrderContextSeed> logger)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeding of database through context {DbContextName} is successfully completed.", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<OrderEntity> GetPreconfiguredOrders()
        {
            return new List<OrderEntity>
            {
                new OrderEntity() {UserName = "swn", FirstName = "Rahul", LastName = "Roy", EmailAddress = "rahulroy.profile@gmail.com", AddressLine = "Bangalore", Country = "India", TotalPrice = 500 }
            };
        }
    }
}
