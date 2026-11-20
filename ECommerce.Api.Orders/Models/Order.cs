namespace ECommerce.Api.Orders.Models;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = "Pending";

    public decimal TotalPrice => Quantity * UnitPrice;
}