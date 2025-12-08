using Application.Common;
using MediatR;

namespace Application.Features.Product.Commands;

public record CreateProductCommand(string ProductName, string? ProductDescription, int Quantity)
    : IRequest<Result<object>>;