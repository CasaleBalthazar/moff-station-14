using Content.Server._Moffstation.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Chat;

namespace Content.Server._Moffstation.Atmos.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class AirPurifierSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmos = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AirPurifierComponent, AtmosDeviceUpdateEvent>(OnAtmosDeviceUpdate);
    }

    private void OnAtmosDeviceUpdate(Entity<AirPurifierComponent> ent, ref AtmosDeviceUpdateEvent ev)
    {
        if (ent.Comp.Air is null)
            return;

        var removed = ent.Comp.Air.RemoveVolume(ent.Comp.Air.Volume);

        var destination = _atmos.GetContainingMixture(ent.Owner);
        if (destination is not null)
        {
            _atmos.ScrubInto(removed, destination, ent.Comp.FilteredGases);
        }
        else
        {
            foreach (var gas in ent.Comp.FilteredGases)
            {
                removed.SetMoles(gas, 0);
            }
        }

        _atmos.Merge(ent.Comp.Air, removed);
    }

    public bool TrySetAir(Entity<AirPurifierComponent?> ent, GasMixture? air)
    {
        if (!Resolve(ent.Owner, ref ent.Comp))
            return false;

        ent.Comp.Air = air;
        return true;
    }
}
