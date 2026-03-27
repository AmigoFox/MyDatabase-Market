using API_DatabaseMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace API_DatabaseMarket.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderItemCountry> OrderItemCountries { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var jsonConverter = new ValueConverter<JsonElement, string>(
                v => v.GetRawText(),
                v => JsonDocument
                        .Parse(v, new JsonDocumentOptions())
                        .RootElement
                        .Clone()
            );


            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.ToTable("exchange_rates");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.CurrencyCode)
                      .HasColumnName("currency_code");

                entity.Property(e => e.RateToRub)
                      .HasColumnName("rate_to_rub");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at");
            });


            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Email)
                      .HasColumnName("email")
                      .IsRequired();

                entity.Property(e => e.PasswordHash)
                      .HasColumnName("password_hash")
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("now()");

                entity.Property(e => e.FullName)
                       .HasColumnName("full_name")
                       .IsRequired();


                entity.Property(e => e.Phone)
                       .HasColumnName("phone")
                        .IsRequired();

                entity.Property(e => e.Login)
                      .HasColumnName("login")
                      .IsRequired();

                entity.Property(e => e.Role)
                      .HasColumnName("role")
                      .HasDefaultValue("user")
                      .IsRequired();

                entity.Property(e => e.IsActive)
                      .HasColumnName("is_active")
                      .HasDefaultValue(true);

                entity.HasIndex(e => e.Login).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();


            });

            // ORDERS
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id");

                entity.Property(e => e.TotalAmount)
                      .HasColumnName("total_amount")
                      .HasColumnType("numeric(10,2)");

                entity.Property(e => e.Status)
                      .HasColumnName("status");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at");

                entity.Property(e => e.OrderName)
                       .HasColumnName("order_name");

                entity.Property(e => e.PaymentDueDate)
                      .HasColumnName("payment_due_date");



                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ORDER_ITEMS
            modelBuilder.Entity<OrderItem>(entity =>
            {

                entity.HasIndex(e => e.DatabaseType);
                entity.HasIndex(e => e.StorageType);
                entity.HasIndex(e => e.Scalability);
                entity.HasIndex(e => e.SizeGB);

                entity.ToTable("order_items");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id")
                      .IsRequired();


                entity.Property(e => e.Config)
                      .HasColumnName("config")
                      .HasColumnType("jsonb")
                      .HasConversion(jsonConverter)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("now()");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.DatabaseType)
                      .HasColumnName("database_type")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.SizeGB)
                      .HasColumnName("size_gb")
                       .IsRequired();

                entity.Property(e => e.Iops)
                      .HasColumnName("iops")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.StorageType)
                      .HasColumnName("storage_type")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Scalability)
                      .HasColumnName("scalability")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.FinalPriceRub)
                      .HasColumnName("final_price_rub")
                      .HasColumnType("numeric(10,2)");
            });


            // PAYMENTS
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id");

                entity.Property(e => e.Amount)
                      .HasColumnName("amount")
                      .HasColumnType("numeric(10,2)");

                entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .HasMaxLength(50);

                entity.Property(e => e.PaymentMethod)
                      .HasColumnName("payment_method");

                entity.Property(e => e.TransactionId)
                      .HasColumnName("transaction_id");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("now()");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Payments )
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Countries
            modelBuilder.Entity<OrderItemCountry>(entity =>
            {
                entity.HasIndex(e => e.CountryCode);
                entity.HasIndex(e => new { e.OrderItemId, e.CountryCode }).IsUnique();

                entity.ToTable("order_item_countries");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.OrderItemId)
                      .HasColumnName("order_item_id")
                      .IsRequired();

                entity.Property(e => e.CountryCode)
                      .HasColumnName("country_code")
                      .HasMaxLength(5)
                      .IsRequired();

                entity.HasOne(e => e.OrderItem)
                      .WithMany(o => o.Countries)
                      .HasForeignKey(e => e.OrderItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
