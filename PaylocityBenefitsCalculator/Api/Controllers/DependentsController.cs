using Api.Abstractions;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _service;

    public DependentsController(IDependentService service)
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    [SwaggerOperation(Summary = "Get dependent by id")]
    [ProducesResponseType(typeof(GetDependentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var response = new ApiResponse<GetDependentDto>();

        try
        {
            var dependent = await _service.Get(id);
            if (dependent != null)
            {
                response.Data = dependent;
                response.Success = true;

                return Ok(response);
            }
            else
            {
                response.Message = $"Dependent with id: {id} was not wound";

                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            response.Error = ex.Message;

            return BadRequest(response);
        }        
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [ProducesResponseType(typeof(List<GetDependentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var response = new ApiResponse<List<GetDependentDto>>();
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
