using CleanArchitecture.Infrastructure.Identity.Models;
using MediatR;

namespace CleanArchitecture.Infrastructure.Identity.Events;

public class UserCreatedEvent : INotification
{
    public ApplicationUser User { get; }

    public UserCreatedEvent(ApplicationUser user)
    {
        User = user;
    }
}

public class UserUpdatedEvent : INotification
{
    public ApplicationUser User { get; }

    public UserUpdatedEvent(ApplicationUser user)
    {
        User = user;
    }
}

public class UserDeletedEvent : INotification
{
    public ApplicationUser User { get; }

    public UserDeletedEvent(ApplicationUser user)
    {
        User = user;
    }
}

public class UserPasswordChangedEvent : INotification
{
    public ApplicationUser User { get; }

    public UserPasswordChangedEvent(ApplicationUser user)
    {
        User = user;
    }
} 