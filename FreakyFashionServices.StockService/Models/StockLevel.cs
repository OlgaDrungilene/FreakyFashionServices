using System.ComponentModel.DataAnnotations;

namespace FreakyFashionServices.StockService.Models;

public class StockLevel
{
   public StockLevel(string sku, int stock)
   {
      Sku = sku;
      Stock = stock;
   }

   [Key]
   public string Sku { get;protected set; }
   public int Stock
   {
      get => stock;
      set
      {
         if (value < 0)
            throw new ArgumentException("Stock can not be less than 0");
         stock = value;
      }
   }

   private int stock;
}
