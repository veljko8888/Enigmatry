using EnigmatryShop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Vendor.Api.Controllers;

namespace ShopTests
{
    internal class ShopApiTestsStartup : Startup
    {
        public ShopApiTestsStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddApplicationPart(typeof(ShopController).Assembly);
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }
}
