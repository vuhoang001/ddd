using Application.Features.User.Commands;
using Application.Features.User.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpDto request)
    {
        var command = new SignUpCommand(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            Password: request.Password
        );
        var result = await mediator.Send(command);
        return Ok(result);
    }
}