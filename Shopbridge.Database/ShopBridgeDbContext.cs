namespace Shopbridge.Database
{
    using Microsoft.EntityFrameworkCore;

    using Shopbridge.Models;

    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopBridgeDbContext).Assembly);
        }
    }
}
