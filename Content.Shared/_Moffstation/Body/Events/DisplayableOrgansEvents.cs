namespace Content.Shared._Moffstation.Body.Events;


/// <summary>
/// Raised on an organ entity when displayed.
/// </summary>
public record struct OrganGotDisplayedEvent(EntityUid Target);

/// <summary>
/// Raised on an organ entity when hidden.
/// </summary>
public record struct OrganGotHiddenEvent(EntityUid Target);
