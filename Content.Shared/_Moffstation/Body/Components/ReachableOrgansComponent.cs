using Content.Shared.Body;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Body.Components;

/// <summary>
/// Used on entities with a <see cref="BodyComponent"/> to indicate that some organs may be inserted and removed
/// from the body.
/// </summary>
[RegisterComponent]
public sealed partial class ReachableOrgansComponent : Component
{
    /// <summary>
    /// The groups of organs that might become reachable inside the body
    /// </summary>
    [DataField]
    public Dictionary<string, ReachableOrganGroup> OrganGroups = new();

    /// <summary>
    /// Sound emitted when inserting an organ in the body
    /// </summary>
    [DataField]
    public SoundSpecifier? InsertSound;

    /// <summary>
    /// Sound emitted when ejecting an organ from the body
    /// </summary>
    [DataField]
    public SoundSpecifier? EjectSound;

    /// <summary>
    /// Organs present in the body, associated to their category.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<OrganCategoryPrototype>, EntityUid> Organs = new();
}

[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class ReachableOrganGroup
{
    /// <summary>
    /// Indicate if the organ group is currently reachable
    /// </summary>
    [DataField]
    public bool Exposed;

    /// <summary>
    /// Categories of organs contained in the group
    /// </summary>
    [DataField]
    public HashSet<ProtoId<OrganCategoryPrototype>> Categories = [];
}
