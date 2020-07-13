using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;
        public ProductsProvider(ProductDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 32.6M, Inventory = 12 });
                dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 12M, Inventory = 12 });
                dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 300M, Inventory = 12 });
                dbContext.Products.Add(new Db.Product() { Id = 4, Name = "MousePad", Price = 10M, Inventory = 12 });
                dbContext.Products.Add(new Db.Product() { Id = 5, Name = "Speaker", Price = 50M, Inventory = 12 });
                dbContext.Products.Add(new Db.Product() { Id = 6, Name = "EarPhone", Price = 40M, Inventory = 12 });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    //var results = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    var results = mapper.Map<IEnumerable<Models.Product>>(products);
                    return (true, results, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

                if(product != null)
                {
                    var result = mapper.Map<Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
        }
    }
}
