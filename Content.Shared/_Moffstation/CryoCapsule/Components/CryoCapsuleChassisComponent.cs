using Content.Shared.Containers.ItemSlots;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CryoCapsuleChassisComponent : Component
{
    /// <summary>
    /// Item slot containing the capsule
    /// </summary>
    [DataField]
    public ItemSlot CapsuleSlot = new();
}
