using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Dtos;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.Responses;

namespace ProductManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public DomainResponse<ProductDto?> GetProductById(int id)
        {
            var response = new DomainResponse<ProductDto?>();
            try
            {
                var product = _productRepository.GetProductById(id);
                if (product is null)
                {
                    response.Message = "Produto não encontrado na base de dados";
                }
                else
                {
                    response.Value = new ProductDto
                    {
                        Id = product.Id,
                        Description = product.Name,
                        Value = product.Cost,
                        Supplier = product.Supplier,
                        Status = product.Active,
                        RegisteredAt = product.RegisteredAt,
                        UpdatedAt = product.ModifiedAt
                    };
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProductById", ex);

                response.Success = false;
                response.Message = $"Ocorreu um erro ao tentar buscar o produto de código {id} na base de dados. " +
                    $"Erro: {ex.Message}";
            }

            return response;
        }

        public DomainCollectionResponse<ProductDto> GetProducts()
        {
            var response = new DomainCollectionResponse<ProductDto>();
            try
            {
                var products = _productRepository.GetProducts();

                response.Values = products.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Description = x.Name,
                    Value = x.Cost,
                    Supplier = x.Supplier,
                    Status = x.Active,
                    RegisteredAt = x.RegisteredAt,
                    UpdatedAt = x.ModifiedAt
                }).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProducts", ex);

                response.Success = false;
                response.Message = "Ocorreu um erro ao tentar buscar a listagem de produtos na base de dados. " +
                    $"Erro: {ex.Message}";
            }

            return response;
        }

        public async Task<GenericResponse> AddProductAsync(ProductDto productDto)
        {
            var response = new GenericResponse();
            try
            {
                var productModel = _mapper.Map<ProductModel>(productDto);

                await _productRepository.AddProductAsync(productModel);

                response.Message = "Produto cadastrado com sucesso";
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("AddProductAsync", ex);

                response.Success = false;
                response.Message = "Ocorreu um erro ao tentar cadastrar o produto na base de dados. " +
                    $"Erro: {ex.Message}";
            }

            return response;
        }
    }
}