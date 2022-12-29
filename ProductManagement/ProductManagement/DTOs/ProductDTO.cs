using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime ManufacturingDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int? SupplierId { get; set; }
        [Required]
        public string? SupplierDescription { get; set; }
        [Required]
        public string? Document { get; set; }
    }
}