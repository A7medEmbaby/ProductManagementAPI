using MediatR;

namespace ProductManagement.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}