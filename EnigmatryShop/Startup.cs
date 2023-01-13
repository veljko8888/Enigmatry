using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Contracts;
using Shared.Models.Models;
using Shop.Application.Core.Interfaces;
using Shop.Application.Models;
using Shop.Infrastructure.Core.Integration;
using Shop.Infrastructure.Core.Repositories;
using Shop.Infrastructure.Core.Services;

namespace EnigmatryShop
{
    public class Startup
    {
        ILogger logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<Startup>();
        }

        private static CachedInventoryRepository CacheService;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            logger.LogInformation("Configure Services started.");

            services.AddControllers();
            services.AddMemoryCache();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IShopService, ShopService>();
            CacheService = new CachedInventoryRepository();
            services.AddSingleton<IInventory>(provider => CacheService);
            services.AddSingleton<IInventoryStore>(provider => CacheService);
            services.AddSingleton<IInventory, WarehouseInventoryRepository>();
            services.AddScoped<IDealerCommunicator, DealerCommunicator>();
            services.AddSingleton<IDb, ShopDb>();
            services.AddSingleton<IAuthService, AuthService>();

            //authentication configuration
            var key = Encoding.UTF8.GetBytes(Configuration["AppSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            logger.LogInformation("Configure Services finished.");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
