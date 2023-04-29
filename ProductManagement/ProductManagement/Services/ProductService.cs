using AutoMapper;
using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;
using ProductManagement.API.ViewModels;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ServiceResponse<ProductViewModel> GetProductById(int id)
        {
            var response = new ServiceResponse<ProductViewModel>();
            try
            {
                var product = _productRepository.GetProductById(id);
                if (product is null)
                {
                    response.Message = "Produto não encontrado na base de dados.";
                }
                else
                {
                    response.Data = new ProductViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Cost = product.Cost,
                        Supplier = product.Supplier,
                        Active = product.Active,
                        RegisteredAt = product.RegisteredAt,
                        ModifiedAt = product.ModifiedAt
                    };
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao buscar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public ServiceCollectionResponse<ProductViewModel> GetProducts()
        {
            var response = new ServiceCollectionResponse<ProductViewModel>();
            try
            {
                var products = _productRepository.GetProducts();
                response.Data = products.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cost = x.Cost,
                    Supplier = x.Supplier,
                    Active = x.Active,
                    RegisteredAt = x.RegisteredAt,
                    ModifiedAt = x.ModifiedAt
                });

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao buscar a lista de produtos na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> AddProductAsync(ProductDTO productDTO)
        {
            var response = new GenericResponse();
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(productDTO);
                await _productRepository.AddProductAsync(productEntity);

                response.Message = "Produto cadastrado com sucesso.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao cadastrar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateProductAsync(ProductDTO productDTO)
        {
            var response = new GenericResponse();
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(productDTO);
                response.Success = await _productRepository.UpdateProductAsync(productEntity);

                response.Message = response.Success ?
                    "Produto atualizado com sucesso." :
                    "Não foi possível atualizar o produto pois o mesmo não foi encontrado na base de dados.";
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao atualizar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> RemoveProductByIdAsync(int id)
        {
            var response = new GenericResponse();
            try
            {
                response.Success = await _productRepository.RemoveProductByIdAsync(id);
                response.Message = response.Success ?
                    "Produto removido com sucesso." :
                    "Não foi possível remover o produto pois o mesmo não foi encontrado na base de dados.";
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao remover o produto da base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }
    }
}