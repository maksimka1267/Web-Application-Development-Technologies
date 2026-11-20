using ECommerce.Api.Customers.Data;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustomersDbContext>(options =>
    options.UseInMemoryDatabase("CustomersDatabase"));

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
        .GetRequiredService<CustomersDbContext>();

    if (!context.Customers.Any())
    {
        context.Customers.AddRange(
            new Customer
            {
                FirstName = "Ivan",
                LastName = "Petrov",
                Email = "ivan@example.com",
                Phone = "+380501112233"
            },
            new Customer
            {
                FirstName = "Alex",
                LastName = "Smith",
                Email = "alex@example.com",
                Phone = "+380671234567"
            }
        );

        context.SaveChanges();
    }
}

app.Run();