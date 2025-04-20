using Api.Abstractions;
using Api.Dtos.Paycheck;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaycheckController : ControllerBase
{
    private readonly IPaycheckCalculationService _service;

    public PaycheckController(IPaycheckCalculationService service)
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    [SwaggerOperation(Summary = "Get employee paycheck by employee id")]
    [ProducesResponseType(typeof(GetPaycheckDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int id, DateTime paycheckDate)
    {
        var response = new ApiResponse<GetPaycheckDto>();
        try
        {
            var paycheck = await _service.Calculate(id, paycheckDate);
            if(paycheck != null)
            {
                response.Data = paycheck;
                response.Success = true;
                return Ok(response);
            }
            else
            {
                response.Message = $"Employee with id: {id} was not wound";
                return NotFound(response);
            }
        }
        catch (Exception ex)
        {
            // TODO: log exception
            response.Error = "Unexpected error occured";
            return BadRequest(response);
        }
    }
}
