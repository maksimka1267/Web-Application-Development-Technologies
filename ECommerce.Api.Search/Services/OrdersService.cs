using System.Text.Json;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services;

public class OrdersService : IOrdersService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrdersService> _logger;

    public OrdersService(
        IHttpClientFactory httpClientFactory,
        ILogger<OrdersService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, IEnumerable<Order>? Orders, string? ErrorMessage)>
        GetOrdersAsync(int customerId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("OrdersService");

            var response = await client.GetAsync($"api/orders/{customerId}");

            if (!response.IsSuccessStatusCode)
            {
                return (false, null, response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsByteArrayAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(
                content,
                options);

            if (orders == null)
            {
                return (false, null, "Orders response is empty");
            }

            return (true, orders, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting orders for customer {CustomerId}", customerId);

            return (false, null, ex.Message);
        }
    }
}