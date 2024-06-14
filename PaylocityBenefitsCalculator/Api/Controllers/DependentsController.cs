using Api.Dtos.Dependent;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Transactions;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var (dependent, errorMessage) = await _dependentService.GetDependentByIdAsync(id);

        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = dependent != null,
            Error = errorMessage,
            Message = errorMessage?? "Dependent found!"
        };

        if (!result.Success) return NotFound(result);

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllDependentsAsync();
        var result = new ApiResponse<List<GetDependentDto>>()
        {
            Data = dependents,
            Success = true
        };

        return Ok(result);
    }
}
