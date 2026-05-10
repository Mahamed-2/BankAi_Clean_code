using Microsoft.EntityFrameworkCore;
using Bankai.Domain.Entities;

namespace Bankai.Infrastructure.Data;

/// <summary>
/// EF Core DbContext — lives in Infrastructure, never referenced by Domain or Application.
/// Application accesses data only through IRepository interfaces.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Product ─────────────────────────────────────────────
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).HasMaxLength(2000);
            entity.Property(p => p.Price).HasPrecision(18, 2);

            // Relationship: Product belongs to Category
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Category ─────────────────────────────────────────────
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Slug).IsRequired().HasMaxLength(120);
            entity.HasIndex(c => c.Slug).IsUnique();
        });

        // ── User ─────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasConversion<int>();
        });

        // ── Order ─────────────────────────────────────────────────
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
            entity.Property(o => o.Status).HasMaxLength(50).HasDefaultValue("Pending");

            entity.HasOne(o => o.User)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── OrderItem ─────────────────────────────────────────────
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            entity.Property(oi => oi.ProductName).IsRequired().HasMaxLength(200);
            entity.Ignore(oi => oi.LineTotal); // Computed property
        });

        // ── Seed Data ─────────────────────────────────────────────
        modelBuilder.Entity<Category>().HasData(
            new { Id = 1, Name = "Electronics",  Slug = "electronics" },
            new { Id = 2, Name = "Clothing",     Slug = "clothing" },
            new { Id = 3, Name = "Books",        Slug = "books" }
        );
    }
}
