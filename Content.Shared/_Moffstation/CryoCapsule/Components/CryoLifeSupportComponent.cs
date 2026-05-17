using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.FixedPoint;
using Content.Shared.MedicalScanner;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CryoLifeSupportComponent : Component
{
    /// <summary>
    /// Item slot containing the capsule
    /// </summary>
    [DataField]
    public ItemSlot CapsuleSlot = new();

    /// <summary>
    /// Item slot containing a beaker for injecting the patient
    /// </summary>
    [DataField]
    public ItemSlot BeakerSlot = new();

    /// <summary>
    /// Sound that will be played when reviving the patient
    /// </summary>
    [DataField]
    public SoundSpecifier? ZapSound = new SoundPathSpecifier("/Audio/Effects/tesla_consume.ogg");

    /// <summary>
    /// Sound that will be played when injecting the patient
    /// </summary>
    [DataField]
    public SoundSpecifier? InjectSound;

    /// <summary>
    /// Time span between two consecutive UI updates
    /// </summary>
    [DataField]
    public TimeSpan UiUpdateInterval = TimeSpan.FromSeconds(0.5);

    [DataField]
    public TimeSpan NextUiUpdate =  TimeSpan.Zero;

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
