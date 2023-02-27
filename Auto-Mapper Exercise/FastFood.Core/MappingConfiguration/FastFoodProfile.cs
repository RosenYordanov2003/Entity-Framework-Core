namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Models;
    using FastFood.Services.Models.Categories;
    using FastFood.Services.Models.Employees;
    using FastFood.Services.Models.Items;
    using ViewModels.Positions;
    using System;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Services.Models.Orders;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                 .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Categories

            this.CreateMap<CategoryDTO, Category>();

            this.CreateMap<CreateCategoryInputModel, CategoryDTO>().
                ForMember(d => d.Name, m => m.MapFrom(s => s.CategoryName));

            this.CreateMap<Category, ListCategoryDto>();

            this.CreateMap<ListCategoryDto, CategoryAllViewModel>();

            //Employees

            this.CreateMap<EmployeeDto, Employee>();

            this.CreateMap<RegisterEmployeeInputModel, EmployeeDto>();

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(d => d.PositionId, m => m.MapFrom(s => s.Id));

            this.CreateMap<Employee, ListEmployeeDto>();

            this.CreateMap<ListEmployeeDto, EmployeesAllViewModel>();

            //Items

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId, m => m.MapFrom(s => s.Id));

            this.CreateMap<CreateItemDto, Item>();

            this.CreateMap<CreateItemInputModel, CreateItemDto>();

            this.CreateMap<Item, ListItemDto>();

            this.CreateMap<ListItemDto, ItemsAllViewModels>();

            //Orders

            //this.CreateMap<CreateItemInputModel, Order>()
            //    .ForMember(d => d.DateTime, m => m.MapFrom(y => DateTime.Now));

            this.CreateMap<Order, ListOrderDto>().
                ForMember(d=>d.OrderId,m=>m.MapFrom(s=>s.Id))
                .ForMember(d=>d.DateTime,m=>m.MapFrom(s=>DateTime.Now));

            this.CreateMap<ListOrderDto, OrderAllViewModel>();
        }
    }
}
