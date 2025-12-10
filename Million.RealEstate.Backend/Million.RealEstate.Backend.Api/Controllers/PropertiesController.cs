using MediatR;
using Microsoft.AspNetCore.Authorization;
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
[Authorize]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new property and returns its unique identifier.
    /// </summary>
    /// <remarks>This method processes the provided <paramref name="request"/> to create a new property
    /// record. The property details are encapsulated in a <see cref="CreatePropertyRequest"/> object. The method
    /// returns a 201 Created response with the location of the newly created resource.</remarks>
    /// <param name="request">The request containing the details of the property to create, including name, address, price, internal code,
    /// year, and owner ID.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the unique identifier of the newly created property.</returns>
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

    /// <summary>
    /// Retrieves a property by its unique identifier.
    /// </summary>
    /// <remarks>This method queries the list of properties and returns the first property that matches the
    /// specified identifier.</remarks>
    /// <param name="id">The unique identifier of the property to retrieve. Must be a positive integer.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the property details as a <see cref="PropertyDto"/> if found;
    /// otherwise, a <see cref="NotFoundResult"/> if no property with the specified identifier exists.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropertyDto>> GetById(int id)
    {
        var list = await _mediator.Send(new GetPropertiesWithFiltersQuery { });
        var property = list.FirstOrDefault(p => p.Id == id);
        if (property is null)
            return NotFound();

        return Ok(property);
    }

    /// <summary>
    /// Updates the price of a property with the specified identifier.
    /// </summary>
    /// <remarks>The <paramref name="id"/> must correspond to an existing property. The <paramref
    /// name="request"/> must contain a valid price value.</remarks>
    /// <param name="id">The unique identifier of the property whose price is to be updated.</param>
    /// <param name="request">An object containing the new price for the property.</param>
    /// <returns>A <see cref="Task{IActionResult}"/> representing the asynchronous operation. Returns <see
    /// cref="NoContentResult"/> if the operation is successful.</returns>
    [HttpPut("{id:int}/price")]
    public async Task<IActionResult> ChangePrice(int id, [FromBody] ChangePriceRequest request)
    {
        var command = new ChangePropertyPriceCommand(id, request.Price);
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Adds an image to the property with the specified identifier.
    /// </summary>
    /// <remarks>The image is associated with the property identified by <paramref name="id"/>.  Ensure that
    /// the provided <paramref name="file"/> is a valid base64-encoded string.</remarks>
    /// <param name="id">The unique identifier of the property to which the image will be added.</param>
    /// <param name="file">The image file content as a base64-encoded string.</param>
    /// <returns>A <see cref="int"/> representing the unique identifier of the newly added image.</returns>
    [HttpPost("{id:int}/images")]
    public async Task<ActionResult<int>> AddImage(int id, [FromBody] string file)
    {
        var command = new AddPropertyImageCommand(id, file);
        var imageId = await _mediator.Send(command);
        return Ok(imageId);
    }

    /// <summary>
    /// Retrieves a collection of properties that match the specified filter criteria.
    /// </summary>
    /// <remarks>All filter parameters are optional. If no filters are provided, the method returns all
    /// available properties.</remarks>
    /// <param name="name">The name or partial name of the property to filter by. This parameter is optional.</param>
    /// <param name="minPrice">The minimum price of the properties to include in the results. This parameter is optional.</param>
    /// <param name="maxPrice">The maximum price of the properties to include in the results. This parameter is optional.</param>
    /// <param name="year">The year associated with the properties to filter by. This parameter is optional.</param>
    /// <param name="ownerId">The unique identifier of the property owner to filter by. This parameter is optional.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="PropertyDto"/> objects
    /// that match the specified filter criteria. If no properties match the criteria, an empty collection is returned.</returns>
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

    /// <summary>
    /// Updates the property with the specified identifier using the provided update request data.
    /// </summary>
    /// <param name="id">The unique identifier of the property to update. Must be a positive integer.</param>
    /// <param name="request">The data used to update the property. This must include the property's name, address, year, and owner ID.</param>
    /// <returns>A <see cref="NoContentResult"/> indicating that the update operation completed successfully.</returns>
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

    /// <summary>
    /// Adds a new property trace for the specified property.
    /// </summary>
    /// <remarks>This method processes the provided trace details and associates them with the specified
    /// property. The <paramref name="id"/> must correspond to an existing property.</remarks>
    /// <param name="id">The unique identifier of the property to which the trace will be added.</param>
    /// <param name="request">The details of the property trace to add, including the date of sale, name, value, and tax.</param>
    /// <returns>The unique identifier of the newly created property trace.</returns>
    [HttpPost("{id:int}/traces")]
    public async Task<ActionResult<int>> AddTrace(int id, [FromBody] AddPropertyTraceRequest request)
    {
        var command = new AddPropertyTraceCommand(id, request.DateSale, request.Name, request.Value, request.Tax);
        var traceId = await _mediator.Send(command);
        return Ok(traceId);
    }
}
