using Content.Shared.Atmos;
using Content.Shared.Body;
using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for entities that can provide limbs for mobs inside a cryocapsule <see cref="CryoCapsuleComponent"/>
/// </summary>
[RegisterComponent]
public sealed partial class CryoCapsuleChassisComponent : Component
{
    /// <summary>
    /// The organs to spawn when installed, based on their category.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<ProtoId<OrganCategoryPrototype>, EntProtoId<OrganComponent>> Organs;

    /// <summary>
    /// The organs present in the host body.
    /// </summary>
    [DataField]
    public List<EntityUid> Presents = new();
}
