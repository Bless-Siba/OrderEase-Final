using Microsoft.EntityFrameworkCore;
using OrderEase.Models;

namespace OrderEase.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        //Configure Relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Using Fluent API for Configuration
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderID); //Configure Primary Key
            
            modelBuilder.Entity<Order>()
                .Property(o => o.Quantity)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryDate)
                .IsRequired();


            modelBuilder.Entity<Order>()
                .Property(o => o.Supplier)
                .IsRequired()
                .HasMaxLength(255);

            //One-to-Many Relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order).HasForeignKey(i => i.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            //Item Configuration
            modelBuilder.Entity<Item>()
                .HasKey(i => i.ItemID);

            modelBuilder.Entity<Item>()
                .Property(i => i.ItemName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            modelBuilder.Entity<Item>()
                .Property(i => i.QuantityInStock)
                .IsRequired();


            base.OnModelCreating(modelBuilder);
        }

        //Defining Tables

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
