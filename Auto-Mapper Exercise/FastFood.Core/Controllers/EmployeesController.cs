namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using FastFood.Models;
    using FastFood.Services.Contracts;
    using FastFood.Services.Models.Employees;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeesController(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Register()
        {
            IList<RegisterEmployeeViewModel> positions = new List<RegisterEmployeeViewModel>();
            ICollection<Position> positionsToConvert = await this.employeeService.GetAllPositions();
            foreach (Position position in positionsToConvert)
            {
                positions.Add(this.mapper.Map<RegisterEmployeeViewModel>(position));
            }
            return this.View(positions);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            EmployeeDto employeeDto = this.mapper.Map<EmployeeDto>(model);
            await this.employeeService.Register(employeeDto);
            return this.RedirectToAction("All", "Employees");
        }

        public async Task<IActionResult> All()
        {
            ICollection<ListEmployeeDto> employees = await this.employeeService.GetAllEmployees();

            IList<EmployeesAllViewModel> employeesToDisplay = new List<EmployeesAllViewModel>();
            foreach (ListEmployeeDto employee in employees)
            {
                EmployeesAllViewModel employeeToAdd = this.mapper.Map<EmployeesAllViewModel>(employee);
                employeesToDisplay.Add(employeeToAdd);
            }
            return this.View(employeesToDisplay);
        }
    }
}
