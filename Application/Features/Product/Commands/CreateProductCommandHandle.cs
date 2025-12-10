using Application.Common;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Product.Commands;

public class CreateProductCommandHandle(
    IRepository<Domain.Entities.Product, int> productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Result<object>>
{
    public async Task<Result<object>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Domain.Entities.Product(request.ProductName, request.ProductDescription, request.Quantity);
        productRepository.Add(product);


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<object>.Success(product);
    }
}