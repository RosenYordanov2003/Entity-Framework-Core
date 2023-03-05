namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTO.Cars;
    using CarDealer.DTO.Customers;
    using CarDealer.DTO.Parts;
    using CarDealer.DTO.Sales;
    using CarDealer.DTO.Suppliers;
    using CarDealer.Models;
    using System.Linq;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDto, Supplier>();

            this.CreateMap<PartDto, Part>();

            this.CreateMap<CustomerDto, Customer>();

            this.CreateMap<SaleDto, Sale>();

            this.CreateMap<Customer, CustomerDto>();

            this.CreateMap<Car, CarInfoDto>();
           
            this.CreateMap<Car,ExportCarDto>();

            this.CreateMap<Part, ExpotPartDto>();

            this.CreateMap<Sale,ExportSaleDto>()
                .ForMember(d=>d.Price,m=>m.MapFrom(s=>s.Car.PartCars.Sum(x=>x.Part.Price)));

            this.CreateMap<Car, ExportCarToyotaDto>();

            this.CreateMap<Supplier, ExportSupplierDto>()
                .ForMember(d=>d.PartsCount,m=>m.MapFrom(s=>s.Parts.Count));
        }
    }
}
