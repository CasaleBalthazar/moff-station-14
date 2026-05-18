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
    /// List of organs that will be added to the mob body upon insertion.
    /// </summary>
    [DataField]
    public List<EntProtoId> Organs = new();

    [DataField]
    public List<EntityUid> Presents = new();
}
