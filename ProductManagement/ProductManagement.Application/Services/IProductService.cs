using ProductManagement.Application.Dtos;
using ProductManagement.Domain.Responses;

namespace ProductManagement.Application.Services
{
    public interface IProductService
    {
        DomainResponse<ProductDto?> GetProductById(int id);
        DomainCollectionResponse<ProductDto> GetProducts();
        Task<GenericResponse> AddProductAsync(ProductDto productDto);
    }
}