namespace ECommerce.Api.Search.Models;

public class CustomerSearchResult
{
    public CustomerDto Customer { get; set; } = new();

    public List<CustomerOrderResult> Orders { get; set; } = new();

    public decimal TotalSpent { get; set; }
}

public class CustomerOrderResult
{
    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public ProductDto? Product { get; set; }
}