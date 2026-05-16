using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.FixedPoint;
using Content.Shared.MedicalScanner;
using Robust.Shared.Serialization;

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
public sealed partial class FitsInCryoLifeSupportComponent : Component;

/// <summary>
/// This is used for entities that are currently inside a cryogenic life support machine.
/// </summary>
[RegisterComponent]
public sealed partial class InsideCryoLifeSupportComponent : Component;


// UI section

[Serializable, NetSerializable]
public enum CryoLifeSupportUiKey : byte
{
    Key,
}


[Serializable, NetSerializable]
public sealed class CryoLifeSupportBuiMessage(
    GasMixEntry? gasMix,
    HealthAnalyzerUiState health,
    FixedPoint2? beakerCapacity,
    List<ReagentQuantity>? beaker)
    : BoundUserInterfaceMessage
{
    public GasMixEntry? GasMix = gasMix;
    public HealthAnalyzerUiState Health = health;
    public FixedPoint2? BeakerCapacity = beakerCapacity;
    public List<ReagentQuantity>? Beaker = beaker;
}

/// <summary>
/// Sent by the UI to perform actions that don't need parameters
/// </summary>
/// <param name="type"></param>
[Serializable, NetSerializable]
public sealed class CryoLifeSupportSimpleUiMessage(CryoLifeSupportSimpleUiMessage.MessageType type)
    : BoundUserInterfaceMessage
{
    public enum MessageType { EjectCapsule, EjectBeaker, ReviveCapsule }

    public readonly MessageType Type = type;
}

/// <summary>
/// Sent by the UI to inject the cryocapsule.
/// </summary>
/// <param name="amount"></param>
[Serializable, NetSerializable]
public sealed class CryoLifeSupportInjectionUiMessage(CryoLifeSupportInjectionUiMessage.MessageDestination destination, int amount)
    : BoundUserInterfaceMessage
{
    public enum MessageDestination { Metabolism, Digestion }

    public readonly MessageDestination Destination = destination;
    public readonly int Amount = amount;
}
