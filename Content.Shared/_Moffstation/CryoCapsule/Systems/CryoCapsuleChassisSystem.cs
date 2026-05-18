using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared.Chat;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoCapsuleChassisSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoCapsuleChassisComponent, InteractUsingEvent>(OnInteractUsing);

        SubscribeLocalEvent<CryoCapsuleComponent, EntInsertedIntoContainerMessage>(OnChassisInserted);
        SubscribeLocalEvent<CryoCapsuleComponent, EntRemovedFromContainerMessage>(OnChassisRemoved);
    }

    // while it look like we insert the capsule inside the chassis in reality the
    // chassis is inserted inside the capsule.
    private void OnInteractUsing(Entity<CryoCapsuleChassisComponent> ent, ref InteractUsingEvent ev)
    {
        if (!TryComp<CryoCapsuleComponent>(ev.Used, out var capsule) ||
            capsule.ChassisSlot.Item is not null)
            return;

        var pos = _transform.GetWorldPosition(ent);

        if (_container.IsEntityInContainer(ev.Used))
            _container.TryRemoveFromContainer(ev.Used);

        if (!_slots.TryInsert(ev.Used, capsule.ChassisSlot, ent.Owner, ev.User))
            return;

        _transform.SetWorldPosition(ev.Used, pos);

    }

    private void OnChassisInserted(Entity<CryoCapsuleComponent> ent, ref EntInsertedIntoContainerMessage ev)
    {
        if (!TryComp<CryoCapsuleChassisComponent>(ev.Entity, out var chassis))
            return;

        foreach (var organ in chassis.Organs)
        {
            PredictedTrySpawnInContainer(organ.Id, ent, "body_organs", out var spawned);
            if (spawned is not null)
                chassis.Presents.Add(spawned.Value);
        }
    }

    private void OnChassisRemoved(Entity<CryoCapsuleComponent> ent, ref EntRemovedFromContainerMessage ev)
    {
        if (!TryComp<CryoCapsuleChassisComponent>(ev.Entity, out var chassis))
            return;

        foreach (var organ in chassis.Presents)
        {
            PredictedDel(organ);
        }

        chassis.Presents = new();
    }
}
