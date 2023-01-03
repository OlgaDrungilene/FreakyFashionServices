namespace FreakyFashionServices.OrderService.Models;

public class Product
{
    public int Id { get; set; }
    public string Sku { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; }


}