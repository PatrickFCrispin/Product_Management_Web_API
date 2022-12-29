using ProductManagement.Domain.Contracts;
using ProductManagement.Domain.Models;
using ProductManagement.Infra.Context;

namespace ProductManagement.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly DBContext _dbContext;

        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProductModel? GetProductById(int id)
        {
            try
            {
                var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);

                return product;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<ProductModel> GetProducts()
        {
            try
            {
                return _dbContext.Products;
            }
            catch (Exception) { throw; }
        }

        public void AddProduct(ProductModel productModel)
        {
            try
            {
                productModel.Status = productModel.Status.ToUpper();

                _dbContext.Add(productModel);
                _dbContext.SaveChanges();
            }
            catch (Exception) { throw; }
        }

        public void UpdateProduct(ProductModel productModel, ProductModel updatedProductModel)
        {
            try
            {
                productModel.Description = updatedProductModel.Description;
                productModel.Status = updatedProductModel.Status.ToUpper();
                productModel.ManufacturingDate = updatedProductModel.ManufacturingDate;
                productModel.ExpirationDate = updatedProductModel.ExpirationDate;
                productModel.SupplierId = updatedProductModel.SupplierId;
                productModel.SupplierDescription = updatedProductModel.SupplierDescription;
                productModel.Document = updatedProductModel.Document;

                _dbContext.Update(productModel);
                _dbContext.SaveChanges();
            }
            catch (Exception) { throw; }
        }

        public void DeactivatedProduct(ProductModel productModel)
        {
            try
            {
                productModel.Status = "INATIVO";

                _dbContext.Update(productModel);
                _dbContext.SaveChanges();
            }
            catch (Exception) { throw; }
        }
    }
}