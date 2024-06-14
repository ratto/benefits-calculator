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

        if (!result.Success) return NotFound(result);

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all employees and their dependents")]
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

    [SwaggerOperation(Summary = "Include a new dependent")]
    [HttpPost("/dependent/{id}")]
    public async Task<ActionResult> AddDependentForEmployee([FromBody] GetDependentDto dependent, int id)
    {
        string errorMessage;
        (var employee, errorMessage) = await _employeeService.GetEmployeeByIdAsync(id);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = employee != null,
            Error = errorMessage,
            Message = errorMessage ?? "Employee found!"
        };

        if (!result.Success) return NotFound(result);

        (var changedEmployee, errorMessage) = _employeeService.PostDependentForEmployee(result.Data!, dependent);

        result = new ApiResponse<GetEmployeeDto>
        {
            Data = changedEmployee,
            Success = changedEmployee != null,
            Error = errorMessage,
            Message = changedEmployee != null ? "Dependent added!" : "Remove the current partner or change the spouse or domestic partner from this employee before adding this dependent."
        };

        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Calculate yearly salary")]
    [HttpGet("/calculate/{id}")]
    public async Task<ActionResult<decimal>> GetPaycheckFromEmployee(int id)
    {
        var (employee, errorMessage) = await _employeeService.GetEmployeeByIdAsync(id);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = employee != null,
            Error = errorMessage,
            Message = errorMessage ?? "Employee found!"
        };

        if (!result.Success) return NotFound(result);

        var paycheck = _employeeService.CalculateEmployeePaycheck(result.Data!);

        return Ok(paycheck);
    }
}
