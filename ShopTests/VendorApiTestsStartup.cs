using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Api;
using Vendor.Api.Controllers;

namespace ShopTests
{
    internal class VendorApiTestsStartup : Startup
    {
        public VendorApiTestsStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddApplicationPart(typeof(DealerOneController).Assembly)
                .AddApplicationPart(typeof(DealerTwoController).Assembly);
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }
}
