using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Models;

namespace ProductManagement.Infra.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlServer("Server=MyServer\\SQLEXPRESS;DataBase=ProductManagementDB;TrustServerCertificate=True;User Id=sa;Password=MyPassword");
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}