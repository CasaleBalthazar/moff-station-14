using Content.Shared.Body;
using Content.Shared.EntityEffects.Effects.EntitySpawning;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Verbs;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.CryoCapsule;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoCapsuleSystem : EntitySystem
{
    [Dependency] private SharedContainerSystem _container = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoCapsuleComponent, ComponentInit>(OnComponentInit);

        // the cryopod have a on examine but turn out since we see the content we can examine it directly
        SubscribeLocalEvent<CryoCapsuleComponent, GetVerbsEvent<AlternativeVerb>>(OnGetAlternativeVerb);
        SubscribeLocalEvent<CryoCapsuleComponent, InteractUsingEvent>(OnInteractUsing);

        SubscribeLocalEvent<CryoCapsuleComponent, LockToggledEvent>(OnLockToggled);
    }

    private void OnComponentInit(Entity<CryoCapsuleComponent> ent, ref ComponentInit args)
    {
        ent.Comp.BodyContainer = _container.EnsureContainer<ContainerSlot>(ent, CryoCapsuleComponent.BodyContainerName);
    }

    private void OnGetAlternativeVerb(Entity<CryoCapsuleComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess|| !args.CanInteract ||
            !ent.Comp.BodyAccessible || ent.Comp.BodyContainer.ContainedEntity is not { } occupant)
            return;

        RaiseLocalEvent(occupant, args);
    }

    private void OnInteractUsing(Entity<CryoCapsuleComponent> ent, ref InteractUsingEvent args)
    {
        if (!ent.Comp.BodyAccessible || ent.Comp.BodyContainer.ContainedEntity is not { } occupant)
            return;

        RaiseLocalEvent(occupant, args);
    }

    private void OnLockToggled(Entity<CryoCapsuleComponent> ent, ref LockToggledEvent args)
    {
        ent.Comp.BodyAccessible = !args.Locked;
    }
}
