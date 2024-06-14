using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.SelectAllEmployeesAsync();
        }

        public async Task<(GetEmployeeDto?, string)> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return (await _employeeRepository.SelectEmployeeAsync(id), "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (GetEmployeeDto?, string) PostDependentForEmployee(GetEmployeeDto employee, GetDependentDto dependent)
        {
            if (employee.Dependents.Where(x => x.Relationship == Models.Relationship.Spouse || x.Relationship == Models.Relationship.DomesticPartner).Any() && (dependent.Relationship == Models.Relationship.Spouse || dependent.Relationship == Models.Relationship.DomesticPartner)) return (null, "Each employee can't have more than one spouse or domestic partner.");

            employee.Dependents.Add(dependent);

            return (employee, "");
        }

        public decimal CalculateEmployeePaycheck(GetEmployeeDto employee)
        {
            decimal deduction = 0.00m;

            decimal employeeDeduction = 1000.00m * 12;

            deduction += employeeDeduction;

            if (employee.Dependents.Count > 0)
            {
                DateTime currentDate = DateTime.Now;

                decimal dependentsDeduction = (employee.Dependents.Count * 600.00m) * 12;

                foreach(var dependent in employee.Dependents)
                {
                    var age = currentDate.Year - dependent.DateOfBirth.Year;

                    if (age >= 50)
                    {
                        dependentsDeduction += (200.00m * 12);
                    }
                }

                deduction += dependentsDeduction;
            }

            var paycheck = (employee.Salary - deduction) / 26;

            if (employee.Salary >= 80000.00m) paycheck -= (employee.Salary * 0.02m);

            return Decimal.Round(paycheck, 2);
        }
    }
}
