﻿using FluentValidation;

namespace ProductManagement.API.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
        public string Supplier { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    internal class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        internal ProductViewModelValidator()
        {
            RuleFor(product => product.Name).NotEmpty().WithMessage("O Nome do produto é obrigatório e deve ser informado.");
            RuleFor(product => product.Cost).GreaterThan(0m).WithMessage("O Preço do produto deve ser maior que 0.");
            RuleFor(product => product.Supplier).NotEmpty().WithMessage("O Fornecedor do produto é obrigatório e deve ser informado.");
        }
    }
}