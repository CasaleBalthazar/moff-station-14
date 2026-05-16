using Content.Shared._Moffstation.Body.Components;
using Content.Shared._Moffstation.Body.Systems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared.Atmos;
using Content.Shared.Lock;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryocapsuleSystem : EntitySystem
{
    [Dependency] private readonly ReachableOrgansSystem _reachableOrgans = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryocapsuleComponent, LockToggledEvent>(OnLockToggled);

        SubscribeLocalEvent<CryocapsuleAirComponent, ComponentStartup>(OnAirStartup);

    }

    private void OnLockToggled(Entity<CryocapsuleComponent> ent, ref LockToggledEvent ev)
    {
        if (ev.Locked)
        {
            _reachableOrgans.TryUnexpose(ent.Owner, CryocapsuleComponent.OrganGroupName);
        }
        else
        {
            _reachableOrgans.TryExpose(ent.Owner, CryocapsuleComponent.OrganGroupName);
        }
    }


    private void OnAirStartup(Entity<CryocapsuleAirComponent> ent, ref ComponentStartup startup)
    {
        ent.Comp.Air = new GasMixture(ent.Comp.Volume);
    }

}
