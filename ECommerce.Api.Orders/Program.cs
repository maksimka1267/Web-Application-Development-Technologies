using ECommerce.Api.Orders.Data;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseInMemoryDatabase("OrdersDatabase"));

var app = builder.Build();

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
        .GetRequiredService<OrdersDbContext>();

    if (!context.Orders.Any())
    {
        context.Orders.AddRange(
            new Order
            {
                CustomerId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 1500,
                OrderDate = DateTime.UtcNow,
                Status = "Completed"
            },
            new Order
            {
                CustomerId = 1,
                ProductId = 2,
                Quantity = 2,
                UnitPrice = 800,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            },
            new Order
            {
                CustomerId = 2,
                ProductId = 3,
                Quantity = 1,
                UnitPrice = 150,
                OrderDate = DateTime.UtcNow,
                Status = "Processing"
            }
        );

        context.SaveChanges();
    }
}

app.Run();