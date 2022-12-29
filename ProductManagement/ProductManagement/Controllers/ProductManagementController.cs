using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.API.Messages;
using ProductManagement.Domain.Contracts;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.Responses;
using ProductManagement.Domain.Validators;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductManagementController : ControllerBase
    {
        readonly IProductRepository _productRepository;

        public ProductManagementController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get-product/id/{id}")] // productmanagement/get-product/id/3
        public ActionResult<ProductDTO> GetProductById(int id)
        {
            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                var genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                return NotFound(genericResponse);
            }

            var response = new Response<ProductDTO>
            {
                Success = true,
                Message = "Ok",
                Value = new ProductDTO
                {
                    Id = productModel.Id,
                    Description = productModel.Description,
                    Status = productModel.Status,
                    ManufacturingDate = productModel.ManufacturingDate,
                    ExpirationDate = productModel.ExpirationDate,
                    SupplierId = productModel.SupplierId,
                    SupplierDescription = productModel.SupplierDescription,
                    Document = productModel.Document,
                }
            };

            return Ok(response);
        }

        [HttpGet("get-products")] // productmanagement/get-products
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = _productRepository.GetProducts();

            if (!products.Any())
            {
                var genericResponse = new GenericResponse
                {
                    Success = true,
                    Message = Messages.ListEmpty
                };

                return Ok(genericResponse);
            }

            var collectionResponse = new CollectionResponse<ProductDTO>
            {
                Success = true,
                Message = "Ok",
                Values = products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Description = x.Description,
                    Status = x.Status,
                    ManufacturingDate = x.ManufacturingDate,
                    ExpirationDate = x.ExpirationDate,
                    SupplierId = x.SupplierId,
                    SupplierDescription = x.SupplierDescription,
                    Document = x.Document
                })
            };

            return Ok(collectionResponse);
        }

        [HttpGet("get-products/page/{page}")] // productmanagement/get-products/page/1  // traz 5 registros por página
        public ActionResult<IEnumerable<ProductDTO>> GetProducts(int page)
        {
            var products = _productRepository.GetProducts().ToList();

            if (!products.Any())
            {
                var genericResponse = new GenericResponse
                {
                    Success = true,
                    Message = Messages.ListEmpty
                };

                return Ok(genericResponse);
            }

            CollectionPaginatedResponse<ProductDTO> collectionPaginatedResponse;
            var limit = 5;
            var valueToCalculateInitialColletionPositionValues = 4;
            var initialPaginateValue = (page * limit) - valueToCalculateInitialColletionPositionValues;

            if (products.Count < initialPaginateValue)
            {
                collectionPaginatedResponse = new CollectionPaginatedResponse<ProductDTO>
                {
                    Success = false,
                    Message = Messages.ListEmptyForPage,
                    Values = new List<ProductDTO> { },
                    Count = products.Count,
                    Limit = limit,
                    Page = page,
                    Pages = 0
                };

                return BadRequest(collectionPaginatedResponse);
            }

            var chunkSize = 5;
            var productsResult = products.Chunk(chunkSize).ToList();
            var chunkArrayPosition = page - 1;

            collectionPaginatedResponse = new CollectionPaginatedResponse<ProductDTO>
            {
                Success = true,
                Message = "Ok",
                Values = productsResult[chunkArrayPosition].Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Description = x.Description,
                    Status = x.Status,
                    ManufacturingDate = x.ManufacturingDate,
                    ExpirationDate = x.ExpirationDate,
                    SupplierId = x.SupplierId,
                    SupplierDescription = x.SupplierDescription,
                    Document = x.Document
                }),
                Count = products.Count,
                Limit = limit,
                Page = page,
                Pages = productsResult.Count
            };

            return Ok(collectionPaginatedResponse);
        }

        [HttpPost("add-product")] // productmanagement/add-product
        public ActionResult AddProduct([FromBody] ProductModel productModel)
        {
            GenericResponse genericResponse;

            var errorMessage = Validator.ValidateFields(productModel);

            if (errorMessage != string.Empty)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = errorMessage
                };

                return BadRequest(genericResponse);
            }

            _productRepository.AddProduct(productModel);

            genericResponse = new GenericResponse
            {
                Success = true,
                Message = Messages.ProductRegisteredSuccessfully
            };

            return Ok(genericResponse);
        }

        [HttpPut("update-product/id/{id}")] // productmanagement/update-product/id/3
        public ActionResult UpdateProduct(int id, [FromBody] ProductModel updatedProductModel)
        {
            GenericResponse genericResponse;

            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                return NotFound(genericResponse);
            }

            var errorMessage = Validator.ValidateFields(updatedProductModel);

            if (errorMessage != string.Empty)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = errorMessage
                };

                return BadRequest(genericResponse);
            }

            _productRepository.UpdateProduct(productModel, updatedProductModel);

            genericResponse = new GenericResponse
            {   
                Success = true,
                Message = Messages.ProductUpdatedSuccessfully
            };

            return Ok(genericResponse);
        }

        // A remoção é lógica, então ao invés de excluir o produto, apenas o inativo na base de dados
        [HttpPut("inactivate-product/id/{id}")] // productmanagement/inactivate-product/id/3
        public ActionResult DeactivatedProduct(int id)
        {
            GenericResponse genericResponse;

            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                return NotFound(genericResponse);
            }

            _productRepository.DeactivatedProduct(productModel);

            genericResponse = new GenericResponse
            {
                Success = true,
                Message = Messages.ProductDeactivatedSuccessfully
            };

            return Ok(genericResponse);
        }
    }
}