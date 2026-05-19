using Content.Shared._Moffstation.StatusEffectNew.Components;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.StatusEffectNew;

/// <summary>
/// This handles...
/// </summary>
public sealed class AddParentStatusEffectSystem : EntitySystem
{
    private readonly IPrototypeManager _proto = IoCManager.Resolve<IPrototypeManager>();
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AddParentStatusEffectComponent, StatusEffectAppliedEvent>(OnEffectApplied);
        SubscribeLocalEvent<AddParentStatusEffectComponent, StatusEffectRemovedEvent>(OnEffectRemoved);
    }

    private void OnEffectApplied(Entity<AddParentStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        foreach (var parent in ent.Comp.Parents)
        {
            var proto = _proto.Resolve(parent, out var prototype);
            if (prototype == null)
                continue;
            EntityManager.AddComponents(args.Target, prototype);
        }
    }

    private void OnEffectRemoved(Entity<AddParentStatusEffectComponent> ent, ref StatusEffectRemovedEvent args)
    {
        foreach (var parent in ent.Comp.Parents)
        {
            var proto = _proto.Resolve(parent, out var prototype);
            if (prototype == null)
                continue;
            EntityManager.RemoveComponents(args.Target, prototype);
        }
    }
}
