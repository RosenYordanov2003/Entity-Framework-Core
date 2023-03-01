namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTO.Customers;
    using CarDealer.DTO.Parts;
    using CarDealer.DTO.Sales;
    using CarDealer.DTO.Suppliers;
    using CarDealer.Models;
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDto, Supplier>();

            this.CreateMap<PartDto, Part>();

            this.CreateMap<CustomerDto, Customer>();

            this.CreateMap<SaleDto, Sale>();
        }
    }
}
