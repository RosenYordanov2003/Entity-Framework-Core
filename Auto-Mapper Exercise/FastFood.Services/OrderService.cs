using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Services.Contracts;
using FastFood.Services.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services
{
    public class OrderService : IOrderService
    {
        private readonly FastFoodContext context;

        private readonly IMapper mapper;
        public OrderService(FastFoodContext contex, IMapper mapper)
        {
            this.context = contex;
            this.mapper = mapper;
        }
        public async Task<ICollection<ListOrderDto>> GetAllOrders()
        {
           ICollection<ListOrderDto> orders = await this.context
                .Orders.ProjectTo<ListOrderDto>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();
            return orders;
        }
    }
}
