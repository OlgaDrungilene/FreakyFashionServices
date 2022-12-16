using FreakyFashionServices.StockService.Models;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.StockService.Data;

public class ApplicationContext : DbContext
{
   public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
   {
   }

   public DbSet<StockLevel> StockLevel { get; set; }
}
