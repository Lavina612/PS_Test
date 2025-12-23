using Microsoft.EntityFrameworkCore;
using PS_Test.Server.Data.Entities;
using PS_Test.Server.Interfaces;

namespace PS_Test.Server.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<ItemEntity> Items { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<OrderStatus>();

            modelBuilder.Entity<CustomerEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ItemEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<OrderEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<OrderEntity>()
                .Property(x => x.Status)
                .HasColumnType("order_status");

            modelBuilder.Entity<ProductEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(x => x.Code)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}