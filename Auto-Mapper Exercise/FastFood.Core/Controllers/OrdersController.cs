namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using FastFood.Models;
    using FastFood.Services.Contracts;
    using FastFood.Services.Models.Orders;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;
        private readonly IOrderService orderService;

        public OrdersController(FastFoodContext context, IMapper mapper, IOrderService orderService)
        {
            this.context = context;
            this.mapper = mapper;
            this.orderService = orderService;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.Select(x => x.Id).ToList(),
                Employees = this.context.Employees.Select(x => x.Id).ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            Order order = new Order { Customer = model.Customer,EmployeeId = model.EmployeeId};
            order.OrderItems.Add(new OrderItem { ItemId = model.ItemId ,Quantity = model.Quantity});

            this.context.Orders.Add(order);
            await this.context.SaveChangesAsync();

            return this.RedirectToAction("All", "Orders");
        }

        public async Task<IActionResult> All()
        {
            IList<OrderAllViewModel> allOrders = new List<OrderAllViewModel>();

            ICollection<ListOrderDto> orders = await this.orderService.GetAllOrders();

            foreach (ListOrderDto order in orders)
            {
                allOrders.Add(this.mapper.Map<OrderAllViewModel>(order));
            }

            return this.View(allOrders);
        }
    }
}
