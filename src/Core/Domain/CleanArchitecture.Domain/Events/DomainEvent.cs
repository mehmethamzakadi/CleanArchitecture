namespace CleanArchitecture.Domain.Events;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; }
    public Guid Id { get; }

    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        Id = Guid.NewGuid();
    }
} 