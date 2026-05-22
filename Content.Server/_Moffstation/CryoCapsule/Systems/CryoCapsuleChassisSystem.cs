using Content.Server._Moffstation.Atmos.Systems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared._Moffstation.CryoCapsule.Events;
using Content.Shared._Moffstation.CryoCapsule.Systems;
using Robust.Shared.Containers;

namespace Content.Server._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoCapsuleChassisSystem : SharedCryoCapsuleChassisSystem
{
    [Dependency] private readonly AirPurifierSystem _purifier = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryoCapsuleChassisComponent, CryoCapsuleChassisGotInstalledEvent>(OnChassisInstalled);
        SubscribeLocalEvent<CryoCapsuleChassisComponent, CryoCapsuleChassisGotRemovedEvent>(OnChassisRemoved);
    }

    private void OnChassisInstalled(Entity<CryoCapsuleChassisComponent> ent, ref CryoCapsuleChassisGotInstalledEvent args)
    {
        if (!TryComp<CryoCapsuleComponent>(args.Capsule, out var capsule))
            return;

        _purifier.TrySetAir(ent.Owner, capsule.Air);
    }

    private void OnChassisRemoved(Entity<CryoCapsuleChassisComponent> ent, ref CryoCapsuleChassisGotRemovedEvent args)
    {
        _purifier.TrySetAir(ent.Owner, null);
    }
}
