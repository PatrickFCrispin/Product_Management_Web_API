using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Contracts;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.Validators;
using ProductManagement.Infra.Context;
using ProductManagement.Infra.Repositories;

// Necessary to be connected on DataBase to run the tests successfully

namespace ProductManagement.Tests
{
    public class ProductManagementTest
    {
        readonly IProductRepository _productRepository;
        readonly ProductModel _productModel;

        public ProductManagementTest()
        {
            _productRepository = new ProductRepository(new DBContext(new DbContextOptions<DBContext>()));

            _productModel = new ProductModel
            {
                Description = "Produto xyz",
                Status = "ATIVO",
                ManufacturingDate = DateTime.Now.AddDays(-3),
                ExpirationDate = DateTime.Now,
                SupplierId = 1,
                SupplierDescription = "Forncedor 1",
                Document = "99.999.999/0001-99"
            };
        }

        [Fact]
        public void OnGetProductById_IfProductWasNotFound_ShouldReturnNull()
        {
            var product = _productRepository.GetProductById(-1);
            
            Assert.Null(product);
        }

        [Fact]
        public void OnGetProducts_IfCollectionIsNotEmpty_ShouldReturnNotEmpty_Otherwise_ShouldReturnEmpty()
        {
            var products = _productRepository.GetProducts();

            if (products.Any())
            {
                Assert.NotEmpty(products);
            }
            else
            {
                Assert.Empty(products);
            }
        }

        [Fact]
        public void OnAddProduct_IfProductFieldsAreNotValid_ShouldReturnErrorMessage()
        {
            _productModel.Description = string.Empty;

            var errorMessage = Validator.ValidateFields(_productModel);

            Assert.True(errorMessage != string.Empty);
        }

        [Fact]
        public void OnAddProduct_IfProductFieldsAreValid_ShouldReturnErrorMessageEqualsStringEmpty()
        {
            var errorMessage = Validator.ValidateFields(_productModel);

            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        public void OnAddProduct_ShouldReturnCounterIncremented()
        {
            var productsCountBeforeAdd = _productRepository.GetProducts().ToList().Count;

            _productRepository.AddProduct(_productModel);

            var productsCountAfterAdd = _productRepository.GetProducts().ToList().Count;

            Assert.True(productsCountAfterAdd > productsCountBeforeAdd);
        }

        [Fact]
        public void OnUpdateProduct_IfProductWasNotFound_ShouldReturnNull_Otherwise_ShouldReturnUpdatedDescription()
        {
            var productModel = _productRepository.GetProductById(1);

            if (productModel is null)
            {
                Assert.Null(productModel);
            }
            else
            {
                _productModel.Description = "Produto Atualizado";

                _productRepository.UpdateProduct(productModel, _productModel);

                productModel = _productRepository.GetProductById(1);

                Assert.Equal("Produto Atualizado", productModel.Description);
            }
        }

        [Fact]
        public void OnDeactivateProduct_IfProductWasNotFound_ShouldReturnNull_Otherwise_ShouldReturnStatusEqualsInativo()
        {
            var productModel = _productRepository.GetProductById(1);

            if (productModel is null)
            {
                Assert.Null(productModel);
            }
            else
            {
                _productRepository.DeactivateProduct(productModel);

                productModel = _productRepository.GetProductById(1);

                Assert.Equal("INATIVO", productModel.Status);
            }
        }
    }
}