using Content.Shared.Inventory;

namespace Content.Shared._Moffstation.Medical.Healing;

/// <summary>
/// The base healing attempt event. It'll be raised on the user and target when attempting to heal the target.
/// </summary>
/// <param name="user">The user who is trying to inject the target.</param>
/// <param name="usedInjector">The injector being used by the user.</param>
/// <param name="target">The target who the user is trying to inject.</param>
/// <param name="overrideMessage">The resulting message that gets displayed per popup.</param>
public abstract partial class BeforeHealTargetEvent(EntityUid user, EntityUid usedInjector, EntityUid target, string? overrideMessage = null)
    : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public EntityUid EntityUsingTopical = user;
    public readonly EntityUid UsedTopical = usedInjector;
    public EntityUid TargetGettingHealed = target;
    public string? OverrideMessage = overrideMessage;
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;
}

/// <summary>
///     This event is raised on the user using the injector before the injector is injected.
///     The event is triggered on the user and all their clothing.
/// </summary>
public sealed class SelfBeforeHealEvent(EntityUid user, EntityUid usedInjector, EntityUid target, string? overrideMessage = null)
    : BeforeHealTargetEvent(user, usedInjector, target, overrideMessage);

/// <summary>
///     This event is raised on the target before the injector is injected.
///     The event is triggered on the target itself and all its clothing.
/// </summary>
[ByRefEvent]
public sealed class TargetBeforeHealEvent(EntityUid user, EntityUid usedInjector, EntityUid target, string? overrideMessage = null)
    : BeforeHealTargetEvent(user, usedInjector, target, overrideMessage);

