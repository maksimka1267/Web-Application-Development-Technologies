using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services;

public class SearchService : ISearchService
{
    private readonly IOrdersService _ordersService;
    private readonly IProductsService _productsService;

    public SearchService(
        IOrdersService ordersService,
        IProductsService productsService)
    {
        _ordersService = ordersService;
        _productsService = productsService;
    }

    public async Task<(bool IsSuccess, dynamic? SearchResults)> SearchAsync(
        int customerId)
    {
        var ordersResult = await _ordersService.GetOrdersAsync(customerId);

        if (!ordersResult.IsSuccess || ordersResult.Orders == null)
        {
            return (false, null);
        }

        var productsResult = await _productsService.GetProductsAsync();

        if (!productsResult.IsSuccess || productsResult.Products == null)
        {
            return (false, null);
        }

        foreach (var order in ordersResult.Orders)
        {
            foreach (var item in order.Items)
            {
                item.ProductName = productsResult.Products
                    .FirstOrDefault(p => p.Id == item.ProductId)
                    ?.Name;
            }
        }

        var result = new
        {
            Orders = ordersResult.Orders
        };

        return (true, result);
    }
}