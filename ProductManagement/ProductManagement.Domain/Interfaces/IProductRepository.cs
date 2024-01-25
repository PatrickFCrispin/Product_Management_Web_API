using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Interfaces
{
    public interface IProductRepository
    {
        ProductEntity? GetProductById(int id);
        IEnumerable<ProductEntity> GetProducts();
        Task AddProductAsync(ProductEntity productEntity);
        Task<bool> UpdateProductAsync(int id, ProductEntity productEntity);
        Task<bool> RemoveProductByIdAsync(int id);
    }
}
