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
    private readonly IEmployeeService _service;
    
    public EmployeesController(IEmployeeService service)
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    [SwaggerOperation(Summary = "Get employee by id")]
    [ProducesResponseType(typeof(GetEmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var response = new ApiResponse<GetEmployeeDto>();
        try
        {
            var employee = await _service.Get(id);
            if (employee != null)
            {
                response.Data = employee;
                response.Success = true;
                return Ok(response);
            }
            else
            {
                response.Message = $"Employee with id: {id} was not wound";
                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            // TODO: log exception
            response.Error = "Unexpected error occured";
            return BadRequest(response);
        }       
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [ProducesResponseType(typeof(List<GetEmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var response = new ApiResponse<List<GetEmployeeDto>>();
        try
        {
            response.Data = await _service.GetAll();
            response.Success = true;

            return response;
        }
        catch (Exception ex) 
        {
            response.Error = ex.Message;

            return BadRequest(response);
        }
    }
}
