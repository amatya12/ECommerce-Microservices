using ECommerce.Api.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrderProvider
    {
        Task<(bool IsSuccess, IEnumerable<OrderModel> Orders, string ErrorMessage)> GetOrdersAsync(int customerId);
    }
}
