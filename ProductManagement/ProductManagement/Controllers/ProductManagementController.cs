using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        readonly ILogger _logger;
        readonly IProductRepository _productRepository;

        public ProductManagementController(ILogger<ProductManagementController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet("get-product/id/{id}")] // productmanagement/get-product/id/3
        public ActionResult<ProductDTO> GetProductById(int id)
        {
            _logger.LogInformation("ProductManagement::GetProductById -> productmanagement/get-product/id/{id}", id);

            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                var genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                _logger.LogWarning("ProductManagement::GetProductById -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

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

            _logger.LogInformation("ProductManagement::GetProductById -> Success: {success}, Message: {Message}, Value: {value}", response.Success, response.Message, JsonConvert.SerializeObject(response.Value));

            return Ok(response);
        }

        [HttpGet("get-products")] // productmanagement/get-products
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            _logger.LogInformation("ProductManagement::GetProducts -> productmanagement/get-products");

            var products = _productRepository.GetProducts();

            if (!products.Any())
            {
                var genericResponse = new GenericResponse
                {
                    Success = true,
                    Message = Messages.ListEmpty
                };

                _logger.LogInformation("ProductManagement::GetProducts -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

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

            _logger.LogInformation("ProductManagement::GetProducts -> Success: {success}, Message: {Message}, Values: {Values}", collectionResponse.Success, collectionResponse.Message, JsonConvert.SerializeObject(collectionResponse.Values));

            return Ok(collectionResponse);
        }

        [HttpGet("get-products/page/{page}")] // productmanagement/get-products/page/1  // traz 5 registros por página
        public ActionResult<IEnumerable<ProductDTO>> GetProducts(int page)
        {
            _logger.LogInformation("ProductManagement::GetProducts -> productmanagement/get-products/page/{page}", page);

            var products = _productRepository.GetProducts().ToList();

            if (!products.Any())
            {
                var genericResponse = new GenericResponse
                {
                    Success = true,
                    Message = Messages.ListEmpty
                };

                _logger.LogInformation("ProductManagement::GetProducts -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

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

                _logger.LogError("ProductManagement::GetProducts -> Success: {Success}, Message: {Message}", collectionPaginatedResponse.Success, collectionPaginatedResponse.Message);

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

            _logger.LogInformation("ProductManagement::GetProducts -> Success: {Success}, Message: {Message}, Values: {Values}", collectionPaginatedResponse.Success, collectionPaginatedResponse.Message, JsonConvert.SerializeObject(collectionPaginatedResponse.Values));

            return Ok(collectionPaginatedResponse);
        }

        [HttpPost("add-product")] // productmanagement/add-product
        public ActionResult AddProduct([FromBody] ProductModel productModel)
        {
            _logger.LogInformation("ProductManagement::AddProduct -> productmanagement/add-product");

            GenericResponse genericResponse;

            var errorMessage = Validator.ValidateFields(productModel);

            if (errorMessage != string.Empty)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = errorMessage
                };

                _logger.LogError("ProductManagement::AddProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

                return BadRequest(genericResponse);
            }

            _productRepository.AddProduct(productModel);

            genericResponse = new GenericResponse
            {
                Success = true,
                Message = Messages.ProductRegisteredSuccessfully
            };

            _logger.LogInformation("ProductManagement::AddProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

            return Ok(genericResponse);
        }

        [HttpPut("update-product/id/{id}")] // productmanagement/update-product/id/3
        public ActionResult UpdateProduct(int id, [FromBody] ProductModel updatedProductModel)
        {
            _logger.LogInformation("ProductManagement::UpdateProduct -> productmanagement/update-product/id/{id}", id);

            GenericResponse genericResponse;

            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                _logger.LogWarning("ProductManagement::UpdateProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

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

                _logger.LogError("ProductManagement::UpdateProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

                return BadRequest(genericResponse);
            }

            _productRepository.UpdateProduct(productModel, updatedProductModel);

            genericResponse = new GenericResponse
            {   
                Success = true,
                Message = Messages.ProductUpdatedSuccessfully
            };

            _logger.LogInformation("ProductManagement::UpdateProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

            return Ok(genericResponse);
        }

        // A remoção é lógica, então ao invés de excluir o produto, apenas o inativo na base de dados
        [HttpPut("deactivate-product/id/{id}")] // productmanagement/deactivate-product/id/3
        public ActionResult DeactivateProduct(int id)
        {
            _logger.LogInformation("ProductManagement::DeactivateProduct -> productmanagement/deactivate-product/id/{id}", id);

            GenericResponse genericResponse;

            var productModel = _productRepository.GetProductById(id);

            if (productModel is null)
            {
                genericResponse = new GenericResponse
                {
                    Success = false,
                    Message = Messages.ProductNotFound(id)
                };

                _logger.LogWarning("ProductManagement::DeactivateProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

                return NotFound(genericResponse);
            }

            _productRepository.DeactivateProduct(productModel);

            genericResponse = new GenericResponse
            {
                Success = true,
                Message = Messages.ProductDeactivatedSuccessfully
            };

            _logger.LogInformation("ProductManagement::DeactivateProduct -> Success: {Success}, Message: {Message}", genericResponse.Success, genericResponse.Message);

            return Ok(genericResponse);
        }
    }
}