namespace ShopBridge.Test
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Shopbridge.Database;

    public class BaseSpecs
    {
        protected ShopBridgeDbContext ShopBridgeDbContext;
        protected ILoggerFactory LoggerFactory;

        protected void SetupDbAndLogger()
        {
            var options = new DbContextOptionsBuilder<ShopBridgeDbContext>()
                .UseInMemoryDatabase(databaseName: "ShopBridgeInMemoryDB")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            ShopBridgeDbContext = new ShopBridgeDbContext(options);
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }
    }
}
