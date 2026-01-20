using Content.Server.Silicons.Laws;
using Content.Shared._Moffstation.Robotics;
using Content.Shared._Moffstation.Robotics.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Silicons.Laws;
using Robust.Server.GameObjects;

namespace Content.Server._Moffstation.Robotics.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class LawConsoleSystem : EntitySystem
{
    /// <inheritdoc/>

    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SiliconLawSystem _lawSystem = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly LawCartridgeSystem _cartridgeSystem = default!;


    private const string CartridgeSlotID = "Cartridge"; // todo : probably put this in the component ?...

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LawConsoleComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<LawConsoleComponent, ComponentRemove>(OnComponentRemove);

        Subs.BuiEvents<LawConsoleComponent>(LawConsoleUiKey.Key, subs =>
            {
                subs.Event<BoundUIOpenedEvent>(OnUIOpened);
                subs.Event<LawConsoleSaveMessage>(OnSaved);
                subs.Event<LawConsoleDeleteMessage>(OnDeleted);
                subs.Event<LawConsoleTransferMessage>(OnTransferred);
            });
    }

    private void OnComponentInit(EntityUid uid, LawConsoleComponent component, ComponentInit args)
    {
        _itemSlotsSystem.AddItemSlot(uid, CartridgeSlotID, component.LawCartridgeSlot);

        foreach (var (name, lawsetId) in component.StandardLawsetsID)
        {
            component.StandardLawsets[name] = _lawSystem.GetLawset(lawsetId);
        }
    }

    private void OnComponentRemove(EntityUid uid, LawConsoleComponent component, ComponentRemove args)
    {
        _itemSlotsSystem.RemoveItemSlot(uid, component.LawCartridgeSlot); // todo : test if i need to remove the cartridge ?
    }

    private void OnUIOpened(EntityUid uid, LawConsoleComponent component, BoundUIOpenedEvent args)
    {
        var state = GetCurrentState(uid, component);
        _uiSystem.SetUiState(uid, LawConsoleUiKey.Key, state);
    }

    private void OnSaved(EntityUid uid, LawConsoleComponent component, LawConsoleSaveMessage args)
    {
        if (component.SavedLawsets.ContainsKey(args.Name) || component.SavedLawsets.ContainsKey(args.Name))
            return;

        component.SavedLawsets[args.Name] = args.Lawset;

        var state = GetCurrentState(uid, component);
        _uiSystem.SetUiState(uid, LawConsoleUiKey.Key, state);
    }

    private void OnDeleted(EntityUid uid, LawConsoleComponent component, LawConsoleDeleteMessage args)
    {
        if (!component.SavedLawsets.ContainsKey(args.Name))
            return;

        component.SavedLawsets.Remove(args.Name);

        var state = GetCurrentState(uid, component);
        _uiSystem.SetUiState(uid, LawConsoleUiKey.Key, state);
    }

    private void OnTransferred(EntityUid uid, LawConsoleComponent component, LawConsoleTransferMessage args)
    {
        if (! component.LawCartridgeSlot.Item.HasValue)
            return;

        if (!TryComp(component.LawCartridgeSlot.Item, out LawCartridgeComponent? cartridge))
            return;


        // i cast because c#.
        _cartridgeSystem.SetName(new((EntityUid)component.LawCartridgeSlot.Item, cartridge), args.Name);
        _cartridgeSystem.SetLawset(new((EntityUid)component.LawCartridgeSlot.Item, cartridge), args.Lawset);

        var state = GetCurrentState(uid, component);
        _uiSystem.SetUiState(uid, LawConsoleUiKey.Key, state);
    }

    private LawConsoleState GetCurrentState(EntityUid uid, LawConsoleComponent component)
    {
        bool isCartridgeInserted = false;
        string cartridgeName = string.Empty;
        SiliconLawset cartridgeLawset = new SiliconLawset();

        if (component.LawCartridgeSlot.Item != null &&
            TryComp(component.LawCartridgeSlot.Item, out LawCartridgeComponent? cartridge))
        {
            isCartridgeInserted = true;
            cartridgeName = cartridge.LawsetName;
            cartridgeLawset = cartridge.Lawset;
        }

        var standardLawsets =  component.StandardLawsets;
        var savedLawsets = component.SavedLawsets;

        return new LawConsoleState(
            isCartridgeInserted,
            cartridgeName,
            cartridgeLawset,
            standardLawsets,
            savedLawsets);
    }
}
