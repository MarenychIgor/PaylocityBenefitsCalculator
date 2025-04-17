using Api.Abstractions;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    
    public EmployeesController(IEmployeeService employeeService)
        => _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));

    [SwaggerOperation(Summary = "Get employee by id")]
    [ProducesResponseType(typeof(GetEmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeService.Get(id);
        
        return employee != null ?
            Ok(employee) :
            NotFound();
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [ProducesResponseType(typeof(List<GetEmployeeDto>), StatusCodes.Status200OK)]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        => new ApiResponse<List<GetEmployeeDto>>
        {
            Data = await _employeeService.GetAll(),
            Success = true
        };
}
