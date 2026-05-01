using Content.Shared.Body;
using Content.Shared.StatusEffect;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.OrgansEffects;

/// <summary>
/// This is used to indicate that the present or absence of certain group of organs in the body
/// will have effects on the mob.
/// </summary>
[RegisterComponent]
public sealed partial class OrgansEffectsComponent : Component
{
    [DataField]
    public List<OrganGroupEffectSpecifier> OrganEffects;

}

[DataDefinition]
public sealed partial class OrganGroupEffectSpecifier
{
    /// <summary>
    /// The organ categories that constitute the group.
    /// </summary>
    public List<ProtoId<OrganCategoryPrototype>> Organs = new();

    /// <summary>
    /// If true the group is considered present only if
    /// all the categories are in.
    /// </summary>
    public bool RequireAll = false;

    /// <summary>
    /// Effects that will be applied when the group is present
    /// </summary>
    public List<ProtoId<StatusEffectPrototype>> OnPresent;

    /// <summary>
    /// Effects that will be applied when the group is absent
    /// </summary>
    public List<ProtoId<StatusEffectPrototype>> OnAbsent;
}
