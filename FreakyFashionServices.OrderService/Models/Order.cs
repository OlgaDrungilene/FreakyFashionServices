namespace FreakyFashionServices.OrderService.Models
{
    public class Order
    {
        
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public  List<Product>? Products { get; set; }

    }
}