using Application.Features.Product.Commands;
using Application.Features.Product.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto product)
    {
        var command = new CreateProductCommand(product.ProductName, product.ProductDescription, product.Quantity);
        var result  = await mediator.Send(command);
        return Ok(result);
    }
}