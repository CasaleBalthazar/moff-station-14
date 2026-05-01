using Content.Shared.FixedPoint;
using Content.Shared.Mobs;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.StatusEffects;

/// <summary>
/// This is used for mobs that cannot live for reasons that cannot be explained with damage numbers.
/// They will be dead regardless of damage while the effect is active.
/// </summary>
[RegisterComponent]
public sealed partial class CannotLiveStatusEffectComponent : Component
{
    /// <summary>
    /// The threshold of the mob that will be restored when the effect is over.
    /// </summary>
    [DataField]
    public Dictionary<FixedPoint2, MobState> SavedThresholds = new Dictionary<FixedPoint2, MobState>();
}
