namespace ShopBridge.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            Application.Configuration.ServicesConfiguration.ConfigureServices(services);
        }
    }
}
