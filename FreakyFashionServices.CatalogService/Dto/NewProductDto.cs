namespace FreakyFashionServices.CatalogService.Dto
{
    public class NewProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
    }
}