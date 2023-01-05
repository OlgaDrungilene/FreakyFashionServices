using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Dto;
using FreakyFashionServices.OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FreakyFashionServices.OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase

{
    private IDistributedCache Cache { get; }



    private ApplicationContext Context;

    public OrdersController(ApplicationContext context, IDistributedCache cache)
    {
        Context = context;
        Cache = cache;
    }
    // POST http://localhost:5003/api/orders
    [HttpPost]

    public async Task<IActionResult> AddOrder(NewOrderDto newOrderDto)
    {

        var serializedBasket = await Cache.GetStringAsync(newOrderDto.Identifier.ToString());

        if (serializedBasket == null)
            return NotFound();//404

        var basket = JsonSerializer.Deserialize<BasketDto>(serializedBasket);

        if (basket == null)
            return UnprocessableEntity();//422

        //TODO
        // 1.Lägga till Items/Products till DB (FK) 
        // 2.newOrder (basket.Items)
        // 3.Lägga till CustomerId till Order (baske Identifier)
        // 4.Remove basket
        // 5. Get method

        var newOrder = new Order
        {
            FirstName = newOrderDto.FirstName,
            LastName = newOrderDto.LastName,
            Email = newOrderDto.Email,
            CreatedAt = DateTime.Now,
            CustomerId = newOrderDto.Identifier
        };

        foreach (var item in basket.Items)
        {
            var product = new Product
            {
                Sku = item.Sku,
                Quantity = item.Quantity,
                Order = newOrder

            };

            Context.Products.Add(product);

        }

        Context.Orders.Add(newOrder);
        Context.SaveChanges();

        var result = new OrderLineDto
        {
            OrderId = newOrder.Id,
            CreatedAt = newOrder.CreatedAt

        };

        Cache.Remove(newOrderDto.Identifier.ToString());

        return Created("", result);


        //TODO
        // 1.Från Reddis - få basket newOrderDto.Identifier
        // 2.Basket kan vara icke existerande (Id) 
        // 3.Deseliarize den
        // 4.Flytta data till DB:Product,OrderLine

    }
    [HttpGet("{customerId}")]

    public IActionResult GetOrder(int customerId)
    {
        var order = Context.Orders.Include(x=>x.Products).FirstOrDefault(x => x.CustomerId == customerId);
        if (order == null)
            return NotFound();

        var result = new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Products = order.Products.Select(x => new ProductDto
            {
                ProductSku = x.Sku,
                Quantity = x.Quantity,

            }).ToList(),


        };
        return Ok(result);
    }

}

