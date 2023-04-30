﻿using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infra.Context;

namespace ProductManagement.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dbContext;

        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProductEntity? GetProductById(int id)
        {
            try
            {
                return _dbContext.Products.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<ProductEntity> GetProducts()
        {
            try
            {
                return _dbContext.Products;
            }
            catch (Exception) { throw; }
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            try
            {
                productEntity.Active = true;
                productEntity.RegisteredAt = productEntity.ModifiedAt = DateTime.Now;

                // aqui não é assíncrono
                _dbContext.Products.Add(productEntity);

                // aqui é, pois acessa o banco de dados subjacente
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<bool> UpdateProductAsync(ProductEntity productEntity)
        {
            try
            {
                var product = GetProductById(productEntity.Id);
                // não preciso 'await' Task.FromResult(false) pois retorno o valor, não tem nada async aqui
                if (product is null) { return Task.FromResult(false).Result; }

                product.Name = productEntity.Name;
                product.Cost = productEntity.Cost;
                product.Supplier = productEntity.Supplier;
                product.Active = productEntity.Active;
                product.ModifiedAt = DateTime.Now;

                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync();

                return Task.FromResult(true).Result;
            }
            catch (Exception) { throw; }
        }

        public async Task<bool> RemoveProductByIdAsync(int id)
        {
            try
            {
                var product = GetProductById(id);
                if (product is null) { return Task.FromResult(false).Result; }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                return Task.FromResult(true).Result;
            }
            catch (Exception) { throw; }
        }
    }
}