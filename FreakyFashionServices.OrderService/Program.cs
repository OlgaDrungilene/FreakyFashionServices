using FreakyFashionServices.OrderService.Converter;
using FreakyFashionServices.OrderService.Data;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationContext>(
           options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            builder.Services.AddControllers()

            .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
             });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["ConnectionStrings:Redis"];
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}