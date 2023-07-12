using ProductService.Data.Model;

namespace BlazorAndRedis.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<bool> CreateProducts(Product product);
    }
}
