using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;
using ProductManagement.API.Services;
using ProductManagement.API.ViewModels;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductManagementController> _logger;

        public ProductManagementController(IProductService productService, IMapper mapper, ILogger<ProductManagementController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")] // products/3
        public ActionResult GetProductById(int id)
        {
            var response = _productService.GetProductById(id);
            if (response.Success)
            {
                if (response.Data is null)
                {
                    _logger.LogInformation("GetProductById::Info -> {message}", response.Message);
                    return NotFound(response);
                }

                _logger.LogInformation(
                    "GetProductById::Info -> Id {id}, Nome {name}, Preço {cost}, Fornecedor {supplier}, Ativo {active}, Cadastrado em {registeredAt}, Editado em {modifiedAt}",
                    response.Data?.Id,
                    response.Data?.Name,
                    response.Data?.Cost,
                    response.Data?.Supplier,
                    response.Data?.Active,
                    response.Data?.RegisteredAt,
                    response.Data?.ModifiedAt);
                return Ok(response);
            }

            _logger.LogError("GetProductById::Error -> {error}", response.Message);
            return BadRequest(response);
        }

        [HttpGet] // products
        public ActionResult GetProducts()
        {
            var response = _productService.GetProducts();
            if (response.Success)
            {
                _logger.LogInformation("GetProducts::Info -> Quantidade de produtos cadastrados {count}", response.Data?.ToList().Count);
                return Ok(response);
            }

            _logger.LogError("GetProducts::Error -> {error}", response.Message);
            return BadRequest(response);
        }


        [HttpPost] // products
        public async Task<ActionResult> AddProductAsync([FromBody] ProductViewModel productViewModel)
        {
            var validator = new ProductViewModelValidator();
            var validationResult = validator.Validate(productViewModel);
            if (validationResult.IsValid)
            {
                var productDTO = _mapper.Map<ProductDTO>(productViewModel);
                var response = await _productService.AddProductAsync(productDTO);
                if (response.Success)
                {
                    _logger.LogInformation("AddProductAsync::Info -> {message}", response.Message);
                    return Ok(response);
                }

                _logger.LogError("AddProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            var errors = ErrorResponse.ToErrorResult(validationResult.Errors);
            return BadRequest(errors);
        }

        [HttpPut] // products
        public async Task<ActionResult> UpdateProductAsync([FromBody] ProductViewModel productViewModel)
        {
            var validator = new ProductViewModelValidator();
            var validationResult = validator.Validate(productViewModel);
            if (validationResult.IsValid)
            {
                var productDTO = _mapper.Map<ProductDTO>(productViewModel);
                var response = await _productService.UpdateProductAsync(productDTO);
                if (response.Success)
                {
                    _logger.LogInformation("UpdateProductAsync::Info -> {message}", response.Message);
                    return Ok(response);
                }

                _logger.LogError("UpdateProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            var errors = ErrorResponse.ToErrorResult(validationResult.Errors);
            return BadRequest(errors);
        }

        [HttpDelete("{id}")] // products/3
        public async Task<ActionResult> RemoveProductByIdAsync(int id)
        {
            var response = await _productService.RemoveProductByIdAsync(id);
            if (response.Success)
            {
                _logger.LogInformation("RemoveProductByIdAsync::Info -> {message}", response.Message);
                return Ok(response);
            }

            _logger.LogError("RemoveProductByIdAsync::Error -> {error}", response.Message);
            return BadRequest(response);
        }
    }
}