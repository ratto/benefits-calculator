using Api.Dtos.Employee;

namespace Api.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<GetEmployeeDto>> SelectAllEmployeesAsync();
    }
}
