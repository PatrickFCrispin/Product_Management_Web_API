namespace ProductManagement.Domain.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}