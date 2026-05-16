using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared.Audio;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public abstract class SharedCryoLifeSupportSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _ambientSound = default!;
    [Dependency] private readonly SharedPointLightSystem _ambientLight = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoLifeSupportComponent, ComponentInit>(OnComponentInit);

        SubscribeLocalEvent<CryoLifeSupportComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
        SubscribeLocalEvent<CryoLifeSupportComponent, EntRemovedFromContainerMessage>(OnEntRemoved);
        SubscribeLocalEvent<CryoLifeSupportComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnComponentInit(Entity<CryoLifeSupportComponent> ent, ref ComponentInit args)
    {
        if (_slots.TryGetSlot(ent, "lifesupport-capsule", out var capSlot))
            ent.Comp.CapsuleSlot = capSlot;

        if (_slots.TryGetSlot(ent, "lifesupport-solution", out var solSlot))
            ent.Comp.BeakerSlot = solSlot;
    }

    private void OnEntInserted(Entity<CryoLifeSupportComponent> ent, ref EntInsertedIntoContainerMessage msg)
    {
        if (msg.Container.ID != ent.Comp.CapsuleSlot?.ID ||
            ! _power.IsPowered(ent.Owner))
            return;

        _ambientSound.SetAmbience(ent, true);
        _ambientLight.SetEnabled(ent, true);
    }

    private void OnEntRemoved(Entity<CryoLifeSupportComponent> ent, ref EntRemovedFromContainerMessage msg)
    {
        if (msg.Container.ID != ent.Comp.CapsuleSlot?.ID ||
            ! _power.IsPowered(ent.Owner))
            return;

        _ambientSound.SetAmbience(ent, false);
        _ambientLight.SetEnabled(ent, false);
    }

    private void OnPowerChanged(Entity<CryoLifeSupportComponent> ent, ref PowerChangedEvent ev)
    {
        var active = ev.Powered && ent.Comp.CapsuleSlot?.Item is not null;
        _ambientSound.SetAmbience(ent, active);
        _ambientLight.SetEnabled(ent, active);
    }
}
