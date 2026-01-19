using Content.Shared.Silicons.Laws;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Robotics;

[Serializable, NetSerializable]
public enum LawConsoleUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class LawConsoleState : BoundUserInterfaceState
{
    public bool IsCartridgeInserted;
    public string CartridgeName;
    public SiliconLawset CartridgeLawset;

    public Dictionary<string, SiliconLawset> StandardLawsets;
    public Dictionary<string, SiliconLawset> SavedLawsets;

    public LawConsoleState (bool isCartridgeInserted, string cartridgeName, SiliconLawset cartridgeLawset,
        Dictionary<string, SiliconLawset> standardLawsets, Dictionary<string, SiliconLawset> savedLawsets)
    {
        IsCartridgeInserted = isCartridgeInserted;
        CartridgeName = cartridgeName;
        CartridgeLawset = cartridgeLawset;
        StandardLawsets = standardLawsets;
        SavedLawsets = savedLawsets;
    }
}

// send when the save button is pressed (to save a new lawset in the console)
[Serializable, NetSerializable]
public sealed class LawConsoleSaveMessage : BoundUserInterfaceMessage
{
    public string Name;
    public SiliconLawset Lawset;
    public EntityUid Target;

    public LawConsoleSaveMessage(string name, SiliconLawset lawset, EntityUid target)
    {
        Name = name;
        Lawset = lawset;
        Target = target;
    }
}

// send when the transfer button is pressed (to transfer a lawset to the law cartridge)
[Serializable, NetSerializable]
public sealed class LawConsoleTransferMessage : BoundUserInterfaceMessage
{
    public string Name;
    public SiliconLawset Lawset;
    public EntityUid Target;

    public LawConsoleTransferMessage(string name, SiliconLawset lawset, EntityUid target)
    {
        Name = name;
        Lawset = lawset;
        Target = target;
    }
}

// send when the delete button is pressed (to delete a saved lawset from the console)
[Serializable, NetSerializable]
public sealed class LawConsoleDeleteMessage : BoundUserInterfaceMessage
{
    public string Name;
    public EntityUid Target;

    public LawConsoleDeleteMessage(string name, EntityUid target)
    {
        Name = name;
        Target = target;
    }
}
