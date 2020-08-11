namespace ShopBridge.Migration
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Shopbridge.Database;

    class Program : IDesignTimeDbContextFactory<ShopBridgeDbContext>
    {
        static void Main(string[] args)
        {
            using (ShopBridgeDbContext shopBridgeDbContext = new Program().CreateDbContext(args))
            {
                shopBridgeDbContext.Database.Migrate();
            }
        }

        public ShopBridgeDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            string connectionString = configuration.GetConnectionString("ShopBridge");

            if (args.Length != 0 && !string.IsNullOrEmpty(args[0]))
                connectionString = args[0].ToString().Trim();

            Console.WriteLine("Connection String to database " + connectionString);

            DbContextOptionsBuilder<ShopBridgeDbContext> optionsBuilder = new DbContextOptionsBuilder<ShopBridgeDbContext>()
                .UseSqlServer(connectionString);

            return new ShopBridgeDbContext(optionsBuilder.Options);
        }
    }
}
