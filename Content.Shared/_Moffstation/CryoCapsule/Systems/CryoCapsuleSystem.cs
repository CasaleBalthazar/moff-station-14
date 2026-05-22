using Content.Shared._Moffstation.Body.Systems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared._Moffstation.CryoCapsule.Events;
using Content.Shared.Atmos;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Lock;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public abstract class SharedCryoCapsuleSystem : EntitySystem
{
    [Dependency] private readonly LockSystem _lock = default!;
    [Dependency] private readonly ItemSlotsSystem _slots= default!;
    [Dependency] private readonly ReachableOrgansSystem _reachableOrgans = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoCapsuleComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<CryoCapsuleComponent, LockToggledEvent>(OnLockToggled);

        SubscribeLocalEvent<CryoCapsuleComponent, CryoCapsuleChassisInstallAttemptEvent>(OnChassisInsertAttempt);
    }

    private void OnComponentInit(Entity<CryoCapsuleComponent> ent, ref ComponentInit args)
    {
        if (_slots.TryGetSlot(ent, "capsule_chassis", out var chassisSlot))
            ent.Comp.ChassisSlot = chassisSlot;

        ent.Comp.Air = new GasMixture(ent.Comp.AirVolume);
    }

    private void OnLockToggled(Entity<CryoCapsuleComponent> ent, ref LockToggledEvent ev)
    {
        if (ev.Locked)
        {
            _reachableOrgans.TryUnexpose(ent.Owner, CryoCapsuleComponent.OrganGroupName);
        }
        else
        {
            _reachableOrgans.TryExpose(ent.Owner, CryoCapsuleComponent.OrganGroupName);
        }
    }

    // todo : raise an event to check if we can insert the organs into the body also
    private void OnChassisInsertAttempt(Entity<CryoCapsuleComponent> ent, ref CryoCapsuleChassisInstallAttemptEvent ev)
    {
        if (ent.Comp.ChassisSlot.HasItem)
        {
            ev.CancelReason = "the capsule is already inside a chassis";
            ev.Cancel();
        }
        else if (_lock.IsLocked(ent.Owner))
        {
            ev.CancelReason = "the capsule must be locked";
            ev.Cancel();
        }
    }
}
