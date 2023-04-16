using FluentValidation;

namespace ProductManagement.Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("A descrição do produto deve ser informada");
            RuleFor(x => x.Value).NotEmpty().GreaterThan(0m).WithMessage("O valor do produto deve ser maior que R$ 0,00");
            RuleFor(x => x.Supplier).NotEmpty().WithMessage("O fornecedor do produto deve ser informado");
            RuleFor(x => x.Status).NotEmpty().Must(BeValidStatus).WithMessage("O status do produto deve ser ATIVO ou INATIVO");
        }

        private bool BeValidStatus(string status)
        {
            if (string.Equals(status, "ATIVO", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(status, "INATIVO", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}