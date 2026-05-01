using Content.Shared.Gibbing;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.HeldTogether;

/// <summary>
/// This handles...
/// </summary>
public sealed class GibWhenOutsideContainerSystem : EntitySystem
{
    [Dependency] private readonly GibbingSystem _gibbing = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GibWhenOutsideContainerComponent, EntGotRemovedFromContainerMessage>(OnExitContainer);
    }

    private void OnExitContainer(Entity<GibWhenOutsideContainerComponent> ent, ref EntGotRemovedFromContainerMessage message)
    {
        _gibbing.Gib(ent, true);
    }
}
