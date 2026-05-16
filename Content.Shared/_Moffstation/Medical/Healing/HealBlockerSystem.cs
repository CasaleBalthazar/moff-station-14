using Content.Shared.Chemistry.Events;
using Content.Shared.Medical;

namespace Content.Shared._Moffstation.Medical.Healing;

/// <summary>
/// This handles...
/// </summary>
public sealed class HealBlockerSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<HealBlockerComponent, TargetBeforeInjectEvent>(OnBeforeInject);
        SubscribeLocalEvent<HealBlockerComponent, TargetBeforeHealEvent>(OnBeforeHeal);
        SubscribeLocalEvent<HealBlockerComponent, TargetBeforeDefibrillatorZapsEvent>(OnBeforeZap);

    }

    private void OnBeforeInject(Entity<HealBlockerComponent> ent, ref TargetBeforeInjectEvent ev)
    {
        ev.Cancel();
    }

    private void OnBeforeHeal(Entity<HealBlockerComponent> ent, ref TargetBeforeHealEvent ev)
    {
        ev.Cancel();
    }

    private void OnBeforeZap(Entity<HealBlockerComponent> ent, ref TargetBeforeDefibrillatorZapsEvent ev)
    {
        ev.Cancel();
    }
}
