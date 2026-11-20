using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(
            OrdersDbContext dbContext,
            ILogger<OrdersProvider> logger,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                // Order 1
                dbContext.Orders.Add(new Db.Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Total = 100,
                    Items = new List<Db.OrderItem>
                    {
                        new Db.OrderItem
                        {
                            Id = 1,
                            ProductId = 1,
                            Quantity = 10,
                            UnitPrice = 10
                        },
                        new Db.OrderItem
                        {
                            Id = 2,
                            ProductId = 2,
                            Quantity = 10,
                            UnitPrice = 10
                        },
                        new Db.OrderItem
                        {
                            Id = 3,
                            ProductId = 3,
                            Quantity = 10,
                            UnitPrice = 10
                        }
                    }
                });

                // Order 2
                dbContext.Orders.Add(new Db.Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Total = 100,
                    Items = new List<Db.OrderItem>
                    {
                        new Db.OrderItem
                        {
                            Id = 4,
                            ProductId = 2,
                            Quantity = 10,
                            UnitPrice = 10
                        }
                    }
                });

                // Order 3
                dbContext.Orders.Add(new Db.Order
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Total = 100,
                    Items = new List<Db.OrderItem>
                    {
                        new Db.OrderItem
                        {
                            Id = 5,
                            ProductId = 3,
                            Quantity = 1,
                            UnitPrice = 100
                        }
                    }
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(
            bool IsSuccess,
            IEnumerable<Models.Order>? Orders,
            string? ErrorMessage
        )> GetOrdersAsync(int customerId)
        {
            try
            {
                logger.LogInformation(
                    "Querying orders for customer {CustomerId}",
                    customerId);

                var orders = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();

                if (orders.Any())
                {
                    var result = mapper.Map<
                        IEnumerable<Db.Order>,
                        IEnumerable<Models.Order>>(orders);

                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while querying orders for customer {CustomerId}",
                    customerId);

                return (false, null, ex.Message);
            }
        }
    }
}