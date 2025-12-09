using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.RealEstate.Backend.Api.Models;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyImage;
using Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyTrace;
using Million.RealEstate.Backend.Application.Properties.Commands.ChangePropertyPrice;
using Million.RealEstate.Backend.Application.Properties.Commands.CreateProperty;
using Million.RealEstate.Backend.Application.Properties.Commands.UpdateProperty;
using Million.RealEstate.Backend.Application.Properties.Queries.GetPropertiesWithFilters;

namespace Million.RealEstate.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePropertyRequest request)
    {
        var command = new CreatePropertyCommand(
            request.Name,
            request.Address,
            request.Price,
            request.CodeInternal,
            request.Year,
            request.OwnerId);

        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropertyDto>> GetById(int id)
    {
        var list = await _mediator.Send(new GetPropertiesWithFiltersQuery { });
        var property = list.FirstOrDefault(p => p.Id == id);
        if (property is null)
            return NotFound();

        return Ok(property);
    }

    [HttpPut("{id:int}/price")]
    public async Task<IActionResult> ChangePrice(int id, [FromBody] ChangePriceRequest request)
    {
        var command = new ChangePropertyPriceCommand(id, request.Price);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/images")]
    public async Task<ActionResult<Guid>> AddImage(int id, [FromBody] string file)
    {
        var command = new AddPropertyImageCommand(id, file);
        var imageId = await _mediator.Send(command);
        return Ok(imageId);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetWithFilters(
        [FromQuery] string? name,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? year,
        [FromQuery] int? ownerId)
    {
        var query = new GetPropertiesWithFiltersQuery
        {
            Name = name,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Year = year,
            OwnerId = ownerId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyRequest request)
    {
        var command = new UpdatePropertyCommand(
            id,
            request.Name,
            request.Address,
            request.Year,
            request.OwnerId);

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id:int}/traces")]
    public async Task<ActionResult<Guid>> AddTrace(int id, [FromBody] AddPropertyTraceRequest request)
    {
        var command = new AddPropertyTraceCommand(id, request.DateSale, request.Name, request.Value, request.Tax);
        var traceId = await _mediator.Send(command);
        return Ok(traceId);
    }
}
