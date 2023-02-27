using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.Models.Employees
{
    public class EmployeeDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public string Address { get; set; }
    }
}
