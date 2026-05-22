using Content.Shared.Body;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.CryoCapsule.Events;


/// <summary>
/// Raised on a cryogenic capsule chassis when attempting to insert a chassis into it
/// </summary>
/// <param name="chassis"> the chassis to be inserted </param>
/// <param name="organs"> the organs that the chassis will install</param>
public sealed class CryoCapsuleChassisInstallAttemptEvent(EntityUid chassis, IEnumerable<ProtoId<OrganCategoryPrototype>> organs) : CancellableEntityEventArgs
{
    public readonly EntityUid Chassis = chassis;
    public readonly IEnumerable<ProtoId<OrganCategoryPrototype>> Organs = organs;
    public string? CancelReason;
}

/// <summary>
/// Raised on a cryogenic capsule chassis when it's inserted in a cryogenic capsule
/// </summary>
/// <param name="Target"></param>
[ByRefEvent]
public readonly record struct CryoCapsuleChassisGotInstalledEvent(EntityUid Capsule);

/// <summary>
/// Raised on a cryogenic capsule chassis when it's removed from a cryogenic capsule
/// </summary>
/// <param name="Target"></param>
[ByRefEvent]
public readonly record struct CryoCapsuleChassisGotRemovedEvent(EntityUid Capsule);
