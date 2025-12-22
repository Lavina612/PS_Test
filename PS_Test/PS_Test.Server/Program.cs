using Microsoft.EntityFrameworkCore;
using Npgsql;
using PS_Test.Server.Data;
using PS_Test.Server.Data.Entities;

namespace PS_Test.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.MapEnum<OrderStatus>("order_status");
            var dataSource = dataSourceBuilder.Build();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(dataSource));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();

                    Console.WriteLine("--> Migrations were successfully applied.");



                    var customer = new CustomerEntity { Name = "John", Code = "aaa", Address = "address", Discount = 5 };
                    context.Customers.Add(customer);
                    context.SaveChanges();

                    var customerId = context.Customers.Where(s => s.Name == "John").First().Id;
                    var order = new OrderEntity { CustomerId = customerId, OrderDate = DateTime.UtcNow, OrderNumber = 1};
                    context.Orders.Add(order);
                    context.SaveChanges();

                    var query = context.Orders.Where(o => o.OrderNumber == 1);

                    foreach (var ord in query)
                    {
                        Console.WriteLine($"Customer: {ord.Customer.Name}, Discount: {ord.Customer.Discount}");
                    }


                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Exception while migrations.");
                }
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
