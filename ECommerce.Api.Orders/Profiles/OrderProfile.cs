using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModel>();
            CreateMap<OrderItem, OrderItemModel>();
        }
       
    }
}
