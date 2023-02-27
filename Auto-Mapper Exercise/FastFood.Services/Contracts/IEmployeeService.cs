using FastFood.Models;
using FastFood.Services.Models.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services.Contracts
{
    public interface IEmployeeService
    {
        Task Register(EmployeeDto employeeDto);

        Task<ICollection<ListEmployeeDto>> GetAllEmployees();

        Task<ICollection<Position>> GetAllPositions();
    }
}
