using Microsoft.EntityFrameworkCore;
using Npgsql;
using PS_Test.Server.Data;
using PS_Test.Server.Data.Entities;
using PS_Test.Server.Interfaces;
using PS_Test.Server.Repositories;
using PS_Test.Server.Services;

namespace PS_Test.Server
{
    public class Program
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.MapEnum<OrderStatus>("order_status");
            var dataSource = dataSourceBuilder.Build();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(dataSource));
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();

                    Console.WriteLine("--> Migrations were successfully applied.");

                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
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
