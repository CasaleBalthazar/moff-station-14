using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.StatusEffectNew.Components;

/// <summary>
/// A component used in correlation with a <see cref="StatusEffectComponent"/> which add a copy of the parent entity
/// components to the entity as long as the effect is active.
/// </summary>
[RegisterComponent]
public sealed partial class AddParentStatusEffectComponent : Component
{
    /// <summary>
    /// Entities that will become parents of the target entity when the effect is active.
    /// </summary>
    [DataField]
    public List<EntProtoId> Parents = new();
}
