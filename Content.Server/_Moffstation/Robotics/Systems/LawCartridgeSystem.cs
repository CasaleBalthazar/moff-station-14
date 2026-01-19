using Content.Server.Silicons.Laws;
using Content.Shared._Moffstation.Robotics.Components;
using Content.Shared.Silicons.Laws;
using Content.Shared.Silicons.Laws.Components;
using Robust.Shared.Prototypes;

namespace Content.Server._Moffstation.Robotics.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class LawCartridgeSystem : EntitySystem
{
    [Dependency] private readonly SiliconLawSystem _siliconLawSystem = default!;
    [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LawCartridgeComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<LawCartridgeComponent, LawCartridgeInstalledEvent>(OnCartridgeInstalled);
        SubscribeLocalEvent<LawCartridgeComponent, LawCartridgeUninstalledEvent>(OnCartridgeUninstalled);
        SubscribeLocalEvent<LawCartridgeComponent, LawCartridgeReplacedEvent>(OnCartridgeReplaced);
    }

    private void OnInit(EntityUid uid, LawCartridgeComponent component, ComponentInit args)
    {
        if (component.LawsetPrototype != string.Empty)
            component.Lawset = _siliconLawSystem.GetLawset(component.LawsetPrototype);
    }


    private void OnCartridgeInstalled(EntityUid uid, LawCartridgeComponent component, LawCartridgeInstalledEvent args)
    {
        var chassis = args.ChassisEnt;
        if (!TryComp<SiliconLawBoundComponent>(chassis, out _))
            return;
        _siliconLawSystem.SetLaws(new(component.Lawset.Laws), chassis, component.LawUploadSound);
    }

    private void OnCartridgeUninstalled(EntityUid uid, LawCartridgeComponent component, LawCartridgeUninstalledEvent args)
    {
        var chassis = args.ChassisEnt;
        if (!TryComp<SiliconLawBoundComponent>(chassis, out var lawComp))
            return;
        component.Lawset = _siliconLawSystem.GetLaws(chassis, lawComp).Clone();
        _siliconLawSystem.SetLaws([], chassis);

    }

    private void OnCartridgeReplaced(EntityUid uid, LawCartridgeComponent component, LawCartridgeReplacedEvent args)
    {
        var chassis = args.ChassisEnt;
        var replacement = args.ReplacementEnt;

        if (!TryComp<SiliconLawBoundComponent>(chassis, out var lawComp))
            return;

        component.Lawset = _siliconLawSystem.GetLaws(chassis, lawComp).Clone();

        if (TryComp<LawCartridgeComponent>(replacement, out var newCartridge))
            _siliconLawSystem.SetLaws(newCartridge.Lawset.Laws, chassis, newCartridge.LawUploadSound);
        else
            _siliconLawSystem.SetLaws([], chassis);
    }


    // Change the name of the lawset present in the cartridge, and the name of the entity.
    public void SetName(Entity<LawCartridgeComponent> ent, string name)
    {
        if (name.Length == 0 || string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name))
            return;

        name = name.Trim();

        ent.Comp.LawsetName = name;

        var metaData = MetaData(ent);

        if (metaData.EntityName.Equals(name, StringComparison.InvariantCulture))
            return;

        _metaDataSystem.SetEntityName(ent, "law cartridge (" + name + ")", metaData);
    }

    public void SetLawset(Entity<LawCartridgeComponent> ent, SiliconLawset siliconLawset)
    {
        ent.Comp.Lawset = siliconLawset;
    }
}
