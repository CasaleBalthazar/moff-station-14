using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.StatusEffectNew;
using Robust.Shared.GameStates;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.StatusEffects;

/// <summary>
/// This handles...
/// </summary>
public sealed class CannotLiveStatusEffectSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CannotLiveStatusEffectComponent, StatusEffectAppliedEvent>(OnStatusEffectApplied);
        SubscribeLocalEvent<CannotLiveStatusEffectComponent, StatusEffectRemovedEvent>(OnStatusEffectRemoved);
    }

    private void OnStatusEffectApplied(Entity<CannotLiveStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        if (!TryComp<MobThresholdsComponent>(args.Target, out var thresholds))
            return;

        ent.Comp.SavedThresholds = new Dictionary<FixedPoint2, MobState>(thresholds.Thresholds);

        var newState = new MobThresholdsComponentState(
            new Dictionary<FixedPoint2, MobState> { { FixedPoint2.Zero, MobState.Dead } },
            thresholds.TriggersAlerts,
            thresholds.CurrentThresholdState,
            thresholds.StateAlertDict,
            thresholds.ShowOverlays,
            thresholds.AllowRevives
        );

        var ev = new ComponentHandleState(newState, newState);
        RaiseLocalEvent(args.Target, ref ev);
    }

    private void OnStatusEffectRemoved(Entity<CannotLiveStatusEffectComponent> ent, ref StatusEffectRemovedEvent args)
    {
        if (!TryComp<MobThresholdsComponent>(args.Target, out var thresholds))
            return;

        var newState = new MobThresholdsComponentState(
            ent.Comp.SavedThresholds,
            thresholds.TriggersAlerts,
            thresholds.CurrentThresholdState,
            thresholds.StateAlertDict,
            thresholds.ShowOverlays,
            thresholds.AllowRevives
        );

        var ev = new ComponentHandleState(newState, newState);
        RaiseLocalEvent(args.Target, ref ev);
    }
}
