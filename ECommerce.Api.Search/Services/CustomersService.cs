using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomerService
    {
        private readonly IHttpClientFactory httpService;
        private readonly ILogger<CustomersService> logger;
        public CustomersService(IHttpClientFactory httpService, ILogger<CustomersService> logger)
        {
            this.httpService = httpService;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, Customer customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var client = httpService.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/customers/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Customer>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
