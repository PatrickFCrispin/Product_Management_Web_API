using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Models;

namespace ProductManagement.Infra.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlServer("Server=Intelinote01\\SQLEXPRESS;DataBase=ProductManagementDB;TrustServerCertificate=True;User Id=sa;Password=Fonsec@2309");
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}