using Microsoft.EntityFrameworkCore;
using PS_Test.Server.Data.Entities;

namespace PS_Test.Server.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<ItemEntity> Items { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
