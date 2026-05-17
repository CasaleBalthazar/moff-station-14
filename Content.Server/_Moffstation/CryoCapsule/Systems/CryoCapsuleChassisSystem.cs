using Content.Server.Atmos.EntitySystems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared._Moffstation.CryoCapsule.Systems;

namespace Content.Server._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoCapsuleChassisSystem : SharedCryoCapsuleChassisSystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryoCapsuleChassisComponent, GetFilterAirEvent>(OnGetFilterAir);
    }

    private void OnGetFilterAir(Entity<CryoCapsuleChassisComponent> ent, ref GetFilterAirEvent ev)
    {
        if (ent.Comp.CapsuleSlot.Item is not { } occupant ||
            !TryComp<CryoCapsuleComponent>(occupant, out var capsule))
            return;

        ev.Air = capsule.Air;
    }
}
