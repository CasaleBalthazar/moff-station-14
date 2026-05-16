using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.StatusEffectNew.Components;

namespace Content.Shared._Moffstation.Mobs.Components;

/// <summary>
/// Force a Mob in the Dead state and make them unrevivable.
/// Use only in conjunction with <see cref="StatusEffectComponent"/>, on the status effect entity.
/// </summary>
[RegisterComponent]
public sealed partial class ForcedDeathStatusEffectComponent : Component
{
    [DataField]
    public Dictionary<FixedPoint2, MobState> Thresholds= new ();
}
