using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/*
 * First steps basic explanation:
 * My first feeling about this seed-project was to refactor it
 * according to some design patern standard. I chose to make a 
 * Clean Architecture with Dependency Injection because I think 
 * it was easier to implement. I'm used to these paterns that 
 * we used in the last company that I worked.
 * The DI also made easier to separate the concerns and make it
 * more SOLID, increasing the implementation speed.
 * 
 * For me, I could have used Factory design patern, but it would
 * be harder for me to test it (as I will explain better in the
 * tests file).
 * 
 * I really think that this first decision on the right design
 * patern really helped me to finish this project within the 
 * first 24h. I think it took me about 9h total of work on this
 * implementation.
 */

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

    /*
     * I was in doubt in where to put this route. At first 
     * I was thinking in creating a new controller and the 
     * whole model for the paycheck and deductions.
     * But then I thougth it would give too much complexity
     * at this time.
     * 
     * I don't like the solution that I made below but with 
     * more time and a better understanding on the business'
     * real needs, I could make a whole new route and give
     * more details about the paycheck and deductions to the
     * front end and give more transparency to the user.
     */

    [SwaggerOperation(Summary = "Calculate monthly paycheck")]
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
