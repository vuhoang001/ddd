using Domain.Events.Product;
using MediatR;

namespace Application.Features.Product.Behavious;

public class CreateProductEventHandler : INotificationHandler<CreateProductEvent>
{
    public Task Handle(CreateProductEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Toi ten la hoang day nha");
        return Task.CompletedTask;
    }
}