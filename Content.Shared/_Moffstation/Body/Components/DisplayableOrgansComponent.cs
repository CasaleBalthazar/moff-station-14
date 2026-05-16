using Content.Shared.Body;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Body.Components;

/// <summary>
/// Used on entity with a <see cref="BodyComponent"/> to indicate that some internal organs may become visible
/// <see cref="DisplayVisualOrganComponent"/>
/// </summary>
[RegisterComponent]
public sealed partial class DisplayableOrgansComponent : Component
{
    /// <summary>
    /// Dictionary containing the different organ groups of the entity
    /// </summary>
    [DataField]
    public Dictionary<string, DisplayableOrganGroup> OrganGroups = new();

    /// <summary>
    /// Organs in the body belonging to at least one group, associated to their type.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<OrganCategoryPrototype>, EntityUid> Organs = new();
}

[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class DisplayableOrganGroup
{
    /// <summary>
    /// Indicate if the organ group is currently visible.
    /// </summary>
    [DataField]
    public bool Displayed;

    /// <summary>
    /// Categories of organs contained in the group
    /// </summary>
    [DataField]
    public HashSet<ProtoId<OrganCategoryPrototype>> Categories = [];
}


/// <summary>
/// Used on organs to apply a sprite to the specified <see cref="Layer"/> within the visible body
/// <see cref="VisualBodyComponent"/> when the organ is displayed <see cref="DisplayableOrgansComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class DisplayVisualOrganComponent : Component
{
    /// <summary>
    /// The sprite layer on the entity that this contributes to
    /// </summary>
    [DataField(required: true)]
    public Enum Layer;

    /// <summary>
    /// The sprite data for the layer
    /// </summary>
    [DataField(required: true), AutoNetworkedField, AlwaysPushInheritance]
    public PrototypeLayerData Data;
}
