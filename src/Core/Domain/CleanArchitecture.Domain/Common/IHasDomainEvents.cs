using CleanArchitecture.Domain.Events;

namespace CleanArchitecture.Domain.Common;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void AddDomainEvent(DomainEvent domainEvent);
    void RemoveDomainEvent(DomainEvent domainEvent);
    void ClearDomainEvents();
} 