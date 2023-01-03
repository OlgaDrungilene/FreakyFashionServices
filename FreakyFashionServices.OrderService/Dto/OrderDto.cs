namespace FreakyFashionServices.OrderService.Dto;

    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public List<ProductDto>? Products { get; set; }
    }
