using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Services;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infra.Context;
using ProductManagement.Infra.Repositories;

namespace ProductManagement.Application.DI
{
    public class Initializer
    {
        public static void Configure(IServiceCollection services, string conectionString)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DBContext>(x => x.UseSqlServer(conectionString, y => y.MigrationsAssembly("ProductManagement.Infra")));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}