using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;
using ProductManagement.API.ViewModels;

namespace ProductManagement.API.Services
{
    public interface IProductService
    {
        ServiceResponse<ProductViewModel> GetProductById(int id);
        ServiceCollectionResponse<ProductViewModel> GetProducts();
        Task<GenericResponse> AddProductAsync(ProductDTO productDTO);
        Task<GenericResponse> UpdateProductAsync(int id, ProductDTO productDTO);
        Task<GenericResponse> RemoveProductByIdAsync(int id);
    }
}