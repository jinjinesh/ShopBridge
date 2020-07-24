namespace ShopBridge.Application.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    using Shopbridge.Database.Repository;
    using ShopBridge.Application.Items;
    using ShopBridge.Contracts.Interfaces;
    using Shopbridge.Database.UnitOfWork;

    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IItemService, ItemService>();
        }
    }
}
