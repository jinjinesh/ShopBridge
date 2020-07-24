namespace ShopBridge.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    using ShopBridge.Application.Configuration;

    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            ShopBridge.Application.Configuration.ServicesConfiguration.ConfigureServices(services);
        }
    }
}
