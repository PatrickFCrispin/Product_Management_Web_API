using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.DTOs;
using ProductManagement.API.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infra.Context;
using ProductManagement.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<ProductManagementDbContext>(x => x.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnectionString"), y => y.MigrationsAssembly("ProductManagement.API")));

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<ProductDTO, ProductEntity>();
});
IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();