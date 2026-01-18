using Content.Shared.Silicons.Laws;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Robotics.Components;


/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class LawCartridgeComponent : Component
{
    /// <summary>
    /// The name of the lawset as displayed in the law console.
    /// </summary>
    [DataField]
    public string LawsetName = string.Empty;

    /// <summary>
    /// The lawset contained in the cartridge.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SiliconLawset Lawset = new();

    /// <summary>
    /// Sound played for the cyborg player when the cartridge is inserted.
    /// </summary>
    [DataField]
    public SoundSpecifier? LawUploadSound = new SoundPathSpecifier("/Audio/Misc/cryo_warning.ogg");

    /// <summary>
    /// ID of the lawset originally present in the cartridge.
    /// </summary>
    [DataField]
    public ProtoId<SiliconLawsetPrototype> LawsetPrototype = string.Empty;
}

// message sent when a cartridge is installed in a borg chassis
[ByRefEvent]
public readonly record struct LawCartridgeInstalledEvent(EntityUid ChassisEnt);

// message sent when a cartridge is uninstalled from a borg chassis
[ByRefEvent]
public readonly record struct LawCartridgeUninstalledEvent(EntityUid ChassisEnt);

// message sent when a cartridge is replaced by another in a borg chassis.
[ByRefEvent]
public readonly record struct LawCartridgeReplacedEvent(EntityUid ChassisEnt, EntityUid ReplacementEnt);
