using freakyfashionservices.orderservice.dto;

namespace FreakyFashionServices.OrderService.Dto;

public class BasketDto
{
    public int Identifier { get; set; }

    public List<ItemDto> Items { get; set; }


}

