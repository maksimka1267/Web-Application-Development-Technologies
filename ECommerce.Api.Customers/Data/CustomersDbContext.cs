using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Data;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(
        DbContextOptions<CustomersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
}