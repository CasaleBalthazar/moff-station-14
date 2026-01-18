using Content.Shared.Containers.ItemSlots;
using Content.Shared.Silicons.Laws;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Robotics.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class LawConsoleComponent : Component
{
    /// <summary>
    /// Slot for a law cartridge
    /// </summary>
    [DataField("cartridge")]
    public ItemSlot LawCartridgeSlot = new();

    /// <summary>
    /// Lawsets present by default in the console edition menu
    /// </summary>
    [DataField]
    public Dictionary<string, SiliconLawset> StandardLawsets = new();

    /// <summary>
    /// Lawsets saved by users in the console edition menu
    /// </summary>
    [DataField]
    public Dictionary<string, SiliconLawset> SavedLawsets = new();

    /// <summary>
    /// IDs of the standard lawsets (by default, the ones commonly found in the AI upload room)
    /// </summary>
    [DataField]
    public Dictionary<string, ProtoId<SiliconLawsetPrototype>> StandardLawsetsID = new()
    {
        { "Crewsimov", "Crewsimov" },
        { "Corporate", "Corporate" },
        { "NT Default", "NTDefault" },
        { "Ten Commandments", "CommandmentsLawset" },
        { "Paladin", "PaladinLawset" },
        { "Live and Let Live", "LiveLetLiveLaws" },
        { "Station Efficiency", "EfficiencyLawset" },
        { "Robocop", "RobocopLawset" },
        { "Game Master", "GameMasterLawset" },
        { "Artist", "PainterLawset" },
        { "Nutimov", "NutimovLawset" },
    };
}
