namespace SoftUni
{
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string result = RemoveTown(context);
            Console.WriteLine(result);
        }
        //3 Employees Full Information Problem
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.Select(e => new { e.FirstName, e.LastName, e.MiddleName, e.JobTitle, e.Salary, e.EmployeeId }).OrderBy(e => e.EmployeeId).ToList();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        //4 Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = context.Employees.Where(e => e.Salary > 50000).OrderBy(e => e.FirstName).ToList();
            foreach (Employee employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        //5 Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.
                Select(e => new { e.FirstName, e.LastName, e.Department.Name, e.Salary }).OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName).
                Where(e => e.Name == "Research and Development").ToList();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        //6 Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            Address address = new Address() { AddressText = "Vitoshka 15", TownId = 4 };
            context.Addresses.Add(address);
            Employee nakov = context.Employees.First(e => e.LastName == "Nakov");
            nakov.Address = address;
            context.SaveChanges();
            string[] addresses = context.Employees.OrderByDescending(e => e.AddressId).
                Take(10).Select(e => e.Address.AddressText).ToArray();
            foreach (string addressText in addresses)
            {
                sb.AppendLine(addressText);
            }
            return sb.ToString().Trim();
        }

        //7 Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                  .Take(10)
                  .Select(e => new
                  {
                      e.FirstName,
                      e.LastName,
                      ManagerFirstName = e.Manager.FirstName,
                      ManagerLastName = e.Manager.LastName,
                      AllEmployeeProjects = e.EmployeesProjects.Select(p => new
                      {
                          p.Project.Name,
                          StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                          EndDate = p.Project.EndDate.HasValue ? p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"

                      }).ToList()
                  }).ToList();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");
                foreach (var project in employee.AllEmployeeProjects)
                {
                    sb.AppendLine($"--{project.Name} - {project.StartDate} - {project.EndDate}");
                }
            }
            return sb.ToString().Trim();
        }

        //8 Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var addresses = context.Addresses.
               OrderByDescending(a => a.Employees.Count).ThenBy(a => a.Town.Name).ThenBy(a => a.AddressText).
                Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                }).Take(10).ToList();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }
            return sb.ToString().Trim();
        }

        //9 Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employee = context.Employees.Where(e => e.EmployeeId == 147).
                Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name).ToList()
                }).First();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.Projects.OrderBy(p => p))
            {
                sb.AppendLine(project);
            }
            return sb.ToString().Trim();
        }
        //10 Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var departments = context.Departments.Where(d => d.Employees.Count > 5).OrderBy(d => d.Employees.Count).ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    DepartmentEmployees = d.Employees.Select(e => new
                    {
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName,
                        EmployeeJobTitle = e.JobTitle
                    }).OrderBy(e => e.EmployeeFirstName).ThenBy(e => e.EmployeeLastName).ToList()
                }).ToList();
            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} – {department.ManagerFirstName} {department.ManagerLastName}");
                foreach (var employee in department.DepartmentEmployees)
                {
                    sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.EmployeeJobTitle}");
                }
            }
            return sb.ToString().Trim();
        }
        //11 Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var projects = context.Projects.OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    ProjectStartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                }).Take(10).ToList();
            foreach (var project in projects.OrderBy(p => p.Name))
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.ProjectStartDate);
            }
            return sb.ToString().Trim();
        }
        //12 Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = new string[4] { "Engineering", "Tool Design", "Marketing", "Information Services" };
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.
                Where(e => e.Department.Name == departments[0] || e.Department.Name == departments[1] || e.Department.Name == departments[2]
                || e.Department.Name == departments[3]).Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    UpdatedSalary = e.Salary + (e.Salary * 12) / 100
                }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.UpdatedSalary:F2})");
            }
            return sb.ToString().Trim();
        }
        //13 Find Employees by First Name Starting With Sa
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.Where(e => e.FirstName.StartsWith("Sa")).
                Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }
            return sb.ToString().Trim();
        }
        //14 Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            Project projectToDelete = context.Projects.First(p => p.ProjectId == 2);
            List<EmployeeProject> employeeProjectsToDelete = context.EmployeesProjects.Where(ep => ep.ProjectId == 2).ToList();
            context.EmployeesProjects.RemoveRange(employeeProjectsToDelete);
            context.SaveChanges();
            context.Projects.Remove(projectToDelete);
            context.SaveChanges();
            StringBuilder sb = new StringBuilder();

            List<string> projectsToGet = context.Projects.Select(p => p.Name).Take(10).ToList();
            foreach (string projectName in projectsToGet)
            {
                sb.AppendLine(projectName);
            }
            return sb.ToString().Trim();
        }

        //15 Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            string townName = "Seattle";
            List<Address> addresses = context.Addresses.Where(a=>a.Town.Name==townName).ToList();
            int countAddressesToDelete = addresses.Count;
            var employees = context.Employees.ToList();
            foreach (var employee in employees)
            {
                if (addresses.Contains(employee.Address))
                {
                    employee.Address = null;
                }
            }
            context.SaveChanges();
            context.Addresses.RemoveRange(addresses);
            Town townToRemove = context.Towns.First(t => t.Name == townName);
            context.SaveChanges();
            return $"{countAddressesToDelete} addresses in Seattle were deleted";
        }
    }
}
