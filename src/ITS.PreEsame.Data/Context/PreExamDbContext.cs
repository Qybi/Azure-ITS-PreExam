using ITS.PreEsame.Models;
using Microsoft.EntityFrameworkCore;

namespace ITS.PreEsame.Data.Context;

public class PreExamDbContext : DbContext
{
    public PreExamDbContext(DbContextOptions<PreExamDbContext> options) : base(options) {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.CustomerId);
    }
}
