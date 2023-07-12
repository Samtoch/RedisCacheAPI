using ProductService.Data;
using ProductService.Data.Model;
using ProductService.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlazorAndRedis.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ProductDbContext _dbContext;
        private IDistributedCache _cache;
        public ProductRepository(ProductDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<List<Product>> GetProducts()
        {
            var cacheData = new List<Product>();
            string recordKey = $"Products_{DateTime.Now:yyyyMMdd_hhmm}";
            cacheData = await _cache.GetRecord<List<Product>>(recordKey);
            if (cacheData == null)
            {
                cacheData = await _dbContext.Products.Take(50).ToListAsync();
                await _cache.SetRecord<List<Product>>(recordKey, cacheData);
            }
            return cacheData;
        }

        public async Task<Product> GetProduct(int id)
        {
            var cacheData = new Product();
            string recordKey = $"Product_Id{id}_{DateTime.Now:yyyyMMdd_hhmm}";
            cacheData = await _cache.GetRecord<Product>(recordKey);
            if (cacheData == null)
            {
                cacheData = await _dbContext.Products.Where(i => i.Id == id).FirstOrDefaultAsync();
                await _cache.SetRecord<Product>(recordKey, cacheData);
            }
            return cacheData;
        }

        public async Task<bool> CreateProducts(Product product)
        {
            try
            {
                await _dbContext.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

       
    }
}
