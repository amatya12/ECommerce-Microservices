using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ECommerce.Api.Search
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IOrderService, OrdersService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ICustomerService, CustomersService>();

            services.AddHttpClient<OrdersService>("OrdersService", config =>
            {
                config.BaseAddress = new Uri("http://localhost:53281");
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

            services.AddHttpClient("ProductsService", config =>
            {
                config.BaseAddress = new Uri("http://localhost:53274");
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5,_ => TimeSpan.FromMilliseconds(500)));

            services.AddHttpClient("CustomersService", config =>
            {
                config.BaseAddress = new Uri("http://localhost:53288");
            }).AddTransientHttpErrorPolicy(c => c.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
        
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
