using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.Contracts;
using FastFood.Services.Models.Employees;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;

        private readonly FastFoodContext fastFoodContext;

        public EmployeeService(IMapper mapper, FastFoodContext fastFoodContext)
        {
            this.mapper = mapper;
            this.fastFoodContext = fastFoodContext;
        }
        public async Task Register(EmployeeDto employeeDto)
        {
            Employee employee = this.mapper.Map<Employee>(employeeDto);
            fastFoodContext.Add(employee);
            await fastFoodContext.SaveChangesAsync();
        }

        public async Task<ICollection<ListEmployeeDto>> GetAllEmployees()
        {
            ICollection<ListEmployeeDto> employees = await this.fastFoodContext.Employees
                .ProjectTo<ListEmployeeDto>(this.mapper.ConfigurationProvider).ToArrayAsync();
            
            return employees;

        }

        public async Task<ICollection<Position>> GetAllPositions()
        {
            ICollection<Position> positions = await fastFoodContext.Positions
                  .ToArrayAsync();
            return positions;
        }
    }
}
