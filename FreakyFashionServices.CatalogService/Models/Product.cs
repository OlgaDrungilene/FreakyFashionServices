using System.ComponentModel.DataAnnotations;

namespace FreakyFashionServices.CatalogService.Models
{
    public class Product
    {
        public Product(string ?name, string description, Uri imageUrl, decimal? price, string ?sku, string urlSlug)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Sku = sku;
            UrlSlug = urlSlug;
        }
        public int Id { get; set; }

        [MaxLength(25)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public Uri ImageUrl { get; set; }
        public decimal? Price { get; set; }
        [MaxLength(10)]
        public string? Sku { get; set; }
        public string UrlSlug { get; set; }
    }
}