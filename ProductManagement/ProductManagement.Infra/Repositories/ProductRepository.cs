using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infra.Context;

namespace ProductManagement.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductManagementDbContext _productManagementDbContext;

        public ProductRepository(ProductManagementDbContext productManagementDbContext)
        {
            _productManagementDbContext = productManagementDbContext;
        }

        public ProductEntity? GetProductById(int id)
        {
            return _productManagementDbContext.Products.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ProductEntity> GetProducts()
        {
            return _productManagementDbContext.Products;
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            productEntity.Active = true;
            productEntity.RegisteredAt = productEntity.ModifiedAt = DateTime.Now;

            _productManagementDbContext.Products.Add(productEntity);

            await _productManagementDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(int id, ProductEntity productEntity)
        {
            var product = GetProductById(id);
            if (product is null) { return false; }

            product.Name = productEntity.Name;
            product.Price = productEntity.Price;
            product.Supplier = productEntity.Supplier;
            product.Active = productEntity.Active;
            product.ModifiedAt = DateTime.Now;

            _productManagementDbContext.Products.Update(product);

            await _productManagementDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveProductByIdAsync(int id)
        {
            var product = GetProductById(id);
            if (product is null) { return false; }

            _productManagementDbContext.Products.Remove(product);

            await _productManagementDbContext.SaveChangesAsync();

            return true;
        }
    }
}
