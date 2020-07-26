namespace ShopBridge
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Shopbridge.Database;
    using ShopBridge.Configuration;

    using Serilog;

    public class Startup
    {
        private const string AllowEverythingCorsPolicyName = "AllowEverything";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(x => x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddSpaStaticFiles(options => options.RootPath = "shopbridge-web/dist");
            services.ConfigureServices();
            services.AddCors(options =>
            {
                options.AddPolicy(AllowEverythingCorsPolicyName,
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    }
                );
            });

            services.ConfigureSwaggerServices();

            services.AddDbContext<ShopBridgeDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("ShopBridge"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(AllowEverythingCorsPolicyName);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSerilogRequestLogging();
            app.ConfigureSwagger();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "shopbridge-web";
                if (env.IsDevelopment())
                {

                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
