using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        
        Task<bool> DeleteProduct(string id);
        Task<bool> UpdateProduct(Product product);
        Task CreateProduct(Product product);

    }

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task CreateProduct(Product product)
        {
            return _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id,id);
            var res=await _context.Products.DeleteOneAsync(filter);
            return res.IsAcknowledged && res.DeletedCount>0;
        }

        public Task<Product> GetProduct(string id)
        {
            return _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            //var filter = Builders<Product>.Filter.Eq(c => c.Category, categoryName);
            //return await _context.Products.FindSync(c => c.Category == categoryName).ToListAsync();
            return await _context.Products.FindSync(c => c.Category == categoryName).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context.Products.FindSync(c => c.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.FindSync(c => true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var res=await _context.Products.ReplaceOneAsync(x=>x.Id==product.Id,product);   
            return res.IsAcknowledged && res.MatchedCount>0;
        }
    }
}
