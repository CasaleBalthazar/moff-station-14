using Content.Shared.Containers.ItemSlots;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CryoLifeSupportComponent : Component
{
    [DataField]
    public ItemSlot? CapsuleSlot;

    [DataField]
    public ItemSlot? BeakerSlot;


}

/// <summary>
/// This is used for entities which can fit inside a cryogenic life support machine.
/// </summary>
[RegisterComponent]
public sealed partial class FitInCryoLifeSupportComponent : Component;

/// <summary>
/// This is used for entities that are currently inside a cryogenic life support machine.
/// </summary>
[RegisterComponent]
public sealed partial class InsideCryoLifeSupportComponent : Component;
