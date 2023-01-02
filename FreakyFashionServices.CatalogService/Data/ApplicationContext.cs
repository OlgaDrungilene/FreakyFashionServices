using FreakyFashionServices.CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.CatalogService.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}

