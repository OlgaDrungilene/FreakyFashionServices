namespace FreakyFashionServices.CatalogService.Dto
{
    internal class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public string Sku { get; set; }
        public string UrlSlug { get; set; }
    }
}