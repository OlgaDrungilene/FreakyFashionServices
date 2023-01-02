using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Dto;
using FreakyFashionServices.OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net.Http.Headers;
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


        // 1. Добавить в БД Items/Products (должна быть связана с Order по FK) 
        // 2. Переложить basket.Items в продукты заказа (newOrder)
        // 3. Добавить в определние Order поле CustomerId (заполнять его Identifier корзиы)
        // 4. Зачистить корзину после успешного создания заказа

        // 5, Перейти к реализации метода GET 

        


        //    var newProduct = new Product
        //{

        //};

        var newOrder = new Order { 
            FirstName=newOrderDto.FirstName,
            LastName=newOrderDto.LastName,
            Email=newOrderDto.Email,
            CreatedAt=DateTime.Now
        };

        Context.Orders.Add(newOrder);
        Context.SaveChanges();

        var result = new OrderLineDto { 
            OrderId=newOrder.Id,
            CreatedAt=newOrder.CreatedAt

        };

        return Created("",result);


        /*
             
       Product
        ================
        * Id
        * OrderId (FK)
        * ProductSKU
        * Quantity
         
         */

       

        // 1. СЛазить в Reddis - получить корзину по кё newOrderDto.Identifier
        // 2. Корзины может не быть по её Id 
        // 3. Десериализуйте её в объектную модель (Deserialize)
        // 4. Переложите данные в БД - должно быть две таблицы:^линия заказ + продукты входящие в заказ
        //      OrderLine (Id, customerId, )


        //return Created();

    }
//    [HttpGet("{sku}")]

//    public IActionResult GetOrder(int identifier)
//    {
//        var order = Context.Orders.FirstOrDefault(x => x.);
//    }
//
}

