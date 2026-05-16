using Content.Shared.Body;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Body.Components;

/// <summary>
/// This is used to add and remove status effect to an entity depending on the absence or presence
/// of certain organ groups in their body.
/// </summary>
[RegisterComponent]
public sealed partial class FunctionalOrgansComponent : Component
{
    /// <summary>
    /// Organ groups contained in the body
    /// </summary>
    [DataField]
    public Dictionary<string, FunctionalOrgansGroup> OrganGroups = new();

    /// <summary>
    /// Indicate what organ categories are currently present inside the body.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<OrganCategoryPrototype>> CategoriesPresent = [];
}

[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class FunctionalOrgansGroup
{
    /// <summary>
    /// Indicate if the organ group is currently present inside the body.
    /// </summary>
    [DataField]
    public bool Present = false;

    /// <summary>
    /// Categories of organs that compose the group
    /// </summary>
    [DataField]
    public HashSet<ProtoId<OrganCategoryPrototype>> Categories = new();

    /// <summary>
    /// List of status effects applied to the entity when all organs of the group are present
    /// </summary>
    [DataField]
    public List<EntProtoId> AppliedWhenPresent = new();

    /// <summary>
    /// List of status effects applied when some organs of the group are missing.
    /// </summary>
    [DataField]
    public List<EntProtoId> AppliedWhenAbsent = new();
}
