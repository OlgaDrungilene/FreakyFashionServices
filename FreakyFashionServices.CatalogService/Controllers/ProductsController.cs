using FreakyFashionServices.CatalogService.Data;
using FreakyFashionServices.CatalogService.Dto;
using FreakyFashionServices.CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.CatalogService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
// POST http://localhost:5001/api/products
{
    private readonly ApplicationContext context;
    public ProductsController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpPost]

    public async Task<IActionResult> AddProduct(NewProductDto newProductDto)
    {
        if (context.Products.Any(x => x.Sku == newProductDto.Sku))
        {
            return Conflict();
        }

        var product = new Product(
            newProductDto.Name,
            newProductDto.Description,
            newProductDto.ImageUrl,
            newProductDto.Price,
            newProductDto.Sku,
            newProductDto.Name.Replace("-", "").Replace(" ", "-").ToLower()

            );

        context.Products.Add(product);
        context.SaveChanges();

        return Created("", product);
    }
    [HttpGet("{sku}")]

    public IActionResult GetProduct(string sku)
    {
        var product = context.Products.FirstOrDefault(x => x.Sku == sku);
        if (product == null)
            return NotFound();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Sku = product.Sku,
            UrlSlug = product.UrlSlug
        };

        return Ok(productDto);

    }

}
