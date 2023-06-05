using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;

namespace ProductManagement.API.Services
{
    public interface IProductService
    {
        ServiceResponse<ProductDTO?> GetProductById(int id);
        ServiceCollectionResponse<ProductDTO> GetProducts();
        Task<GenericResponse> AddProductAsync(ProductDTO productDTO);
        Task<GenericResponse> UpdateProductAsync(int id, ProductDTO productDTO);
        Task<GenericResponse> RemoveProductByIdAsync(int id);
    }
}