using ProductManagement.Domain.Models;

namespace ProductManagement.Domain.Validators
{
    public static class Validator
    {
        public static string ValidateFields(ProductModel productModel)
        {
            if (InvalidDescription(productModel.Description))
            {
                return "O campo Description (descrição) é obrigatório";
            }

            if (InvalidStatus(productModel.Status))
            {
                return "O campo Status deve ser ATIVO ou INATIVO";
            }

            if (InvalidManufacturingDate(productModel.ManufacturingDate, productModel.ExpirationDate))
            {
                return "O campo ManufacturingDate (data de fabricação) deve ser menor que o campo ExpirationDate (data de expiração)";
            }

            if (InvalidSupplierDescription(productModel.SupplierDescription))
            {
                return "O campo SupplierDescription (descrição do fornecedor) é obrigatório";
            }

            if (InvalidDocument(productModel.Document))
            {
                return "O campo Document (CNPJ) é obrigatório";
            }

            return string.Empty;
        }

        static bool InvalidDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description);
        }

        static bool InvalidStatus(string status)
        {
            if (!string.Equals(status, "ATIVO", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(status, "INATIVO", StringComparison.OrdinalIgnoreCase)) { return true; }
            }

            return false;
        }

        static bool InvalidManufacturingDate(DateTime manufacturingDate, DateTime expirationDate)
        {
            if (manufacturingDate >= expirationDate) { return true; }

            return false;
        }

        static bool InvalidSupplierDescription(string supplierDescription)
        {
            return string.IsNullOrWhiteSpace(supplierDescription);
        }

        static bool InvalidDocument(string document)
        {
            return string.IsNullOrWhiteSpace(document);
        }
    }
}