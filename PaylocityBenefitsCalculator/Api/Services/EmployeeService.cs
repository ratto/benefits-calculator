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
    }
}
