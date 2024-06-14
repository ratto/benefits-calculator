using Api.Dtos.Dependent;
using Api.Dtos.Employee;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
        Task<(GetEmployeeDto?, string)> GetEmployeeByIdAsync(int id);
        (GetEmployeeDto?, string) PostDependentForEmployee(GetEmployeeDto employee, GetDependentDto dependent);
        decimal CalculateEmployeePaycheck(GetEmployeeDto employee);
    }
}
