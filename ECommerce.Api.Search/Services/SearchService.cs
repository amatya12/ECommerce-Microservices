using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductsService productsService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductsService productsService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await orderService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            var customerResult = await customerService.GetCustomerAsync(customerId);
            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ? productsResult.products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product Name not available";
                    }
                }


                var result = new
                {
                    CustomerName = customerResult.IsSuccess ? customerResult.customer.Name : "Customer Name not found",
                    CustomerAddress = customerResult.IsSuccess ? customerResult.customer.Address : "Customer Address not found",
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
