using Content.Shared._Moffstation.Mobs.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.StatusEffectNew;
using Robust.Shared.GameStates;

namespace Content.Shared._Moffstation.Mobs.Systems;

/// <remarks>
/// In many ways, the work of the critic is an easy one. We do not risk anything, yet we enjoy a position
/// of superiority compared to those subjecting themselves, with their work, to our judgement. With that being said,
/// FUCK YOU <see cref="MobThresholdsComponent"/>.
/// </remarks>

/// <summary>
/// This handles...
/// </summary>
public sealed class ForcedDeathSystem : EntitySystem
{
    [Dependency] private readonly MobThresholdSystem _thresholds = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ForcedDeathStatusEffectComponent, StatusEffectAppliedEvent>(OnEffectApplied);
        SubscribeLocalEvent<ForcedDeathStatusEffectComponent, StatusEffectRemovedEvent>(OnEffectRemoved);
    }

    private void OnEffectApplied(Entity<ForcedDeathStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        if (! TryComp<MobStateComponent>(args.Target, out var mobState) ||
            ! TryComp<MobThresholdsComponent>(args.Target, out var thresholds) ||
            Terminating(args.Target))
            return;

        ent.Comp.Thresholds = new Dictionary<FixedPoint2, MobState>(thresholds.Thresholds);
        _thresholds.SetMobStateThreshold(args.Target, FixedPoint2.Zero, MobState.Dead);
        _thresholds.VerifyThresholds(args.Target);
    }

    private void OnEffectRemoved(Entity<ForcedDeathStatusEffectComponent> ent, ref StatusEffectRemovedEvent args)
    {
        if (Terminating(args.Target))
            return;

        foreach (var (threshold, state) in ent.Comp.Thresholds)
        {
            _thresholds.SetMobStateThreshold(args.Target, threshold, state);
        }
    }
}
