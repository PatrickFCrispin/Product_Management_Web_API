using ProductManagement.Domain.Models;

namespace ProductManagement.Domain.Contracts
{
    public interface IProductRepository
    {
        ProductModel? GetProductById(int id);
        IEnumerable<ProductModel> GetProducts();
        void AddProduct(ProductModel productModel);
        void UpdateProduct(ProductModel productModel, ProductModel updatedProductModel);
        void DeactivatedProduct(ProductModel productModel);
    }
}