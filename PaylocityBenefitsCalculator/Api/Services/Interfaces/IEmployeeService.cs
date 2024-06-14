using Api.Dtos.Employee;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
        Task<(GetEmployeeDto?, string)> GetEmployeeByIdAsync(int id);
        decimal CalculateEmployeePaycheck(GetEmployeeDto employee);
    }
}
