using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private NpgsqlConnection Connection => new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
       
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affectedRows = await Connection.ExecuteAsync("INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@ProductName, @Description, @Amount)", new { coupon.ProductName, coupon.Description, coupon.Amount });
            if(affectedRows == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affectedRows = await Connection.ExecuteAsync("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id", new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            if (affectedRows == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var affectedRows = await Connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName=@ProductName", new { productName});
            if (affectedRows == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await Connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName=@ProductName", new { productName });
            if (coupon == null)
            {
                return new Coupon() { ProductName = "No Product",Description = "No Desc", Amount = 0};
            }
            return coupon;
        }        
    }
}
