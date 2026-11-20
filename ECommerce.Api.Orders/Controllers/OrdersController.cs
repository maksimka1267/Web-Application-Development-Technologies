using ECommerce.Api.Orders.Data;
using ECommerce.Api.Orders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;

    public OrdersController(OrdersDbContext context)
    {
        _context = context;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    // GET: api/orders/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        return order;
    }

    // GET: api/orders/customer/1
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(
        int customerId)
    {
        var orders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        return orders;
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        if (order.CustomerId <= 0 || order.ProductId <= 0 ||
            order.Quantity <= 0 || order.UnitPrice < 0)
        {
            return BadRequest("Invalid order data.");
        }

        order.Id = 0;
        order.OrderDate = DateTime.UtcNow;

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetOrder),
            new { id = order.Id },
            order
        );
    }

    // PUT: api/orders/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(
        int id,
        Order updatedOrder)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        if (updatedOrder.CustomerId <= 0 ||
            updatedOrder.ProductId <= 0 ||
            updatedOrder.Quantity <= 0 ||
            updatedOrder.UnitPrice < 0)
        {
            return BadRequest("Invalid order data.");
        }

        order.CustomerId = updatedOrder.CustomerId;
        order.ProductId = updatedOrder.ProductId;
        order.Quantity = updatedOrder.Quantity;
        order.UnitPrice = updatedOrder.UnitPrice;
        order.Status = updatedOrder.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/orders/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}