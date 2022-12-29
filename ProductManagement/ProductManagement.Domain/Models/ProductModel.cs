namespace ProductManagement.Domain.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierDescription { get; set; }
        public string Document { get; set; }
    }
}