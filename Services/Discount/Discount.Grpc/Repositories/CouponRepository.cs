using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }

    public class DiscountRepository : IDiscountRepository
    {
        IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = GetConnection();
            var commadnText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)";
            var res = await connection.ExecuteAsync(commadnText, new { coupon.ProductName, coupon.Description, coupon.Amount });
            if (res > 0)
                return true;
            return false;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = GetConnection();
            var commadnText = "DELETE FROM Coupon WHERE ProductName = @ProductName";
            var res = await connection.ExecuteAsync(commadnText, new { ProductName = productName });
            if (res > 0)
                return true;
            return false;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = GetConnection();
            var comandText = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(comandText, new { ProductName = productName });

            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = GetConnection();
            var commadnText = "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id";
            var res = await connection.ExecuteAsync(commadnText, new { coupon.ID, coupon.ProductName, coupon.Description, coupon.Amount });
            if (res > 0)
                return true;
            return false;
        }
    }
}
