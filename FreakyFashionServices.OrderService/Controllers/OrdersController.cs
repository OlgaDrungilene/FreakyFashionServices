using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Dto;
using FreakyFashionServices.OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace FreakyFashionServices.OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase

{
    private ApplicationContext Context;

    private readonly IHttpClientFactory ClientFactory;

    public OrdersController(ApplicationContext context, IHttpClientFactory clientFactory)
    {
        Context = context;
        ClientFactory = clientFactory;
    }

    private async Task<bool> DeleteBasket(int identifier)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:5002/api/baskets/" + identifier);

        var client = ClientFactory.CreateClient();

        var response = await client.SendAsync(request);

        return (response.StatusCode == HttpStatusCode.OK);

    }

    private async Task<BasketDto?> GetBasket(int identifier)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5002/api/baskets/" + identifier);

        request.Headers.Add("Accept", "application/json");

        var client = ClientFactory.CreateClient();

        var response = await client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        var responseStream = await response.Content.ReadAsStreamAsync();

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        var basketDto = await JsonSerializer.DeserializeAsync<BasketDto>(responseStream, options);

        return basketDto;
    }
    // POST http://localhost:5003/api/orders
    [HttpPost]

    public async Task<IActionResult> AddOrder(NewOrderDto newOrderDto)
    {
        var basket= await GetBasket(newOrderDto.Identifier);
        if (basket == null)
            return NotFound();

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

        await DeleteBasket(newOrderDto.Identifier);

        return Created("", result);

    }
    [HttpGet("{customerId}")]

    public IActionResult GetOrder(int customerId)
    {
        var order = Context.Orders.Include(x => x.Products).FirstOrDefault(x => x.CustomerId == customerId);
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

