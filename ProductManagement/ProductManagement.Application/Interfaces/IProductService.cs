using ProductManagement.Application.DTOs;
using ProductManagement.Application.Responses;

namespace ProductManagement.Application.Interfaces
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
