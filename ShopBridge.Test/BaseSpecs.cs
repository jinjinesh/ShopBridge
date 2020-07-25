namespace ShopBridge.Test
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Shopbridge.Database;

    using NUnit.Framework;

    public class BaseSpecs
    {
        protected ShopBridgeDbContext shopBridgeDbContext;
        protected ILoggerFactory loggerFactory;

        protected void SetupDbAndLogger()
        {
            var options = new DbContextOptionsBuilder<ShopBridgeDbContext>()
                .UseInMemoryDatabase(databaseName: "ShopBridgeInMemoryDB")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            shopBridgeDbContext = new ShopBridgeDbContext(options);
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }
    }
}
