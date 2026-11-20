using ECommerce.Api.Products.Data;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDatabase"));

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Начальные данные
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ProductsDbContext>();

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product
            {
                Name = "Laptop",
                Description = "Gaming laptop",
                Price = 1500,
                Stock = 10
            },
            new Product
            {
                Name = "Smartphone",
                Description = "Modern smartphone",
                Price = 800,
                Stock = 20
            },
            new Product
            {
                Name = "Headphones",
                Description = "Wireless headphones",
                Price = 150,
                Stock = 30
            }
        );

        context.SaveChanges();
    }
}

app.Run();