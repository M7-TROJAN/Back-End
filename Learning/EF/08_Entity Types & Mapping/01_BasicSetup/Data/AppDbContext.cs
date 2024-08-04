using _01_BasicSetup.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _01_BasicSetup.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .ToTable("Products", schema: "Inventory")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Order>()
                .ToTable("Orders", schema: "Sales")
                .HasKey(O => O.Id);

            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetails", schema:"Sales")
                .HasKey(od => new { od.OrderId, od.ProductId }); // Composite key

            // modelBuilder.HasDefaultSchema("Sales"); // Default schema for all tables


            // Define relationships
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
