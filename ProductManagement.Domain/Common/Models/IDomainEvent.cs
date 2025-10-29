using MediatR;

namespace ProductManagement.Domain.Common.Models;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}