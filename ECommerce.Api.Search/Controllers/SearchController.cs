using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace ECommerce.Api.Search.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SearchController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: api/search/customer/1
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<CustomerSearchResult>> SearchCustomer(
        int customerId)
    {
        if (customerId <= 0)
            return BadRequest("Invalid customer ID.");

        try
        {
            var customerClient =
                _httpClientFactory.CreateClient("Customers");

            var ordersClient =
                _httpClientFactory.CreateClient("Orders");

            var productsClient =
                _httpClientFactory.CreateClient("Products");

            // Получаем покупателя
            var customer = await customerClient
                .GetFromJsonAsync<CustomerDto>(
                    $"api/customers/{customerId}");

            if (customer == null)
                return NotFound("Customer not found.");

            // Получаем заказы покупателя
            var orders = await ordersClient
                .GetFromJsonAsync<List<OrderDto>>(
                    $"api/orders/customer/{customerId}");

            orders ??= new List<OrderDto>();

            var result = new CustomerSearchResult
            {
                Customer = customer
            };

            // Получаем товары для каждого заказа
            foreach (var order in orders)
            {
                var product = await productsClient
                    .GetFromJsonAsync<ProductDto>(
                        $"api/products/{order.ProductId}");

                result.Orders.Add(new CustomerOrderResult
                {
                    OrderId = order.Id,
                    Quantity = order.Quantity,
                    UnitPrice = order.UnitPrice,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    OrderDate = order.OrderDate,
                    Product = product
                });

                result.TotalSpent += order.TotalPrice;
            }

            return Ok(result);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                "One or more microservices are unavailable."
            );
        }
        catch (TaskCanceledException)
        {
            return StatusCode(
                StatusCodes.Status504GatewayTimeout,
                "Microservice request timed out."
            );
        }
    }
}