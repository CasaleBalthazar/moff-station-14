using Content.Shared._Moffstation.Body.Components;
using Content.Shared._Moffstation.Body.Systems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared.Atmos;
using Content.Shared.Lock;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public abstract class SharedCryoCapsuleSystem : EntitySystem
{
    [Dependency] private readonly ReachableOrgansSystem _reachableOrgans = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoCapsuleComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<CryoCapsuleComponent, LockToggledEvent>(OnLockToggled);
    }

    private void OnComponentInit(Entity<CryoCapsuleComponent> ent, ref ComponentInit args)
    {
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
}
