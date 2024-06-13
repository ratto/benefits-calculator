using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var (employee, errorMessage) = await _employeeService.GetEmployeeByIdAsync(id);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = employee != null,
            Error = errorMessage,
            Message = errorMessage ?? "Employee found!"
        };

        if (result.Data == null) return NotFound(result);
        
        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }
}
