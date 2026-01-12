using Microsoft.EntityFrameworkCore;
using API_DatabaseMarket.Models;

namespace API_DatabaseMarket.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Таблицы
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("now()");
            });

            // ORDERS
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TotalAmount)
                      .HasColumnType("numeric(10,2)");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("now()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ORDER_ITEMS
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Config)
                      .HasColumnType("jsonb");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("now()");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PAYMENTS
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                      .HasColumnType("numeric(10,2)");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("now()");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
