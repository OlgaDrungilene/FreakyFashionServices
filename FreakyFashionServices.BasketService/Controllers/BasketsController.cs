using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FreakyFashionServices.BasketService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    public IDistributedCache Cache { get; }

    public BasketsController(IDistributedCache cache)
    {
        Cache = cache;
    }

    [HttpPut("{identifier}")] //customer number

    public async Task<IActionResult> UpdateBasket(int identifier, BasketDto basket)
    {
        if (identifier != basket.Identifier)

            return BadRequest("Customer Id mismatch");

        var serializedBaskets = await Cache.GetStringAsync(identifier.ToString());

        await Cache.SetStringAsync(
      identifier.ToString(),
      JsonSerializer.Serialize(basket));

        if (serializedBaskets != null)

            return NoContent();

        return Created("", null);


    }


    [HttpGet("{identifier}")] //GET http://localhost:5000/api/baskets/1

    public async Task<ActionResult<BasketDto>> GetBasket(int identifier)
    {
        var serializedBasket = await Cache.GetStringAsync(identifier.ToString());

        if (serializedBasket == null)
            return NotFound();//404

        var basket = JsonSerializer.Deserialize<BasketDto>(serializedBasket);

        if (basket == null)
            return UnprocessableEntity();//422

        var result = new BasketDto
        {
            Identifier = basket.Identifier,
            Items = basket.Items.Select(x => new ItemDto
            {
                Sku = x.Sku,
                Quantity = x.Quantity
            }).ToList()
        };
        return Ok(result);
    }

    [HttpDelete("{identifier}")] // DELETE http://localhost:5000/api/baskets/1

    public async Task<IActionResult> DeleteBasket(int identifier)

    {
        var serializedBasket = await Cache.GetStringAsync(identifier.ToString());

        if (serializedBasket == null)
            return NoContent();//204

        Cache.Remove(identifier.ToString());

        return Ok();//200
    }


}

public class BasketDto
{
    public int Identifier { get; set; }

    public List<ItemDto> Items { get; set; }


}


public class ItemDto
{
    public string Sku { get; set; }
    public int Quantity { get; set; }
}