namespace Content.Shared._Moffstation.Body.Events;

/// <summary>
/// Raised on an organ entity when they become exposed and are reachable
/// </summary>
public record struct OrganGotExposedEvent(EntityUid Target);

/// <summary>
/// Raised on an organ entity when they are no longer exposed and reachable.
/// </summary>
public record struct OrganGotUnexposedEvent(EntityUid Target);

