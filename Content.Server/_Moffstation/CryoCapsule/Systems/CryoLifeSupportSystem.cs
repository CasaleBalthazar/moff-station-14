using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Server.Medical;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared._Moffstation.CryoCapsule.Systems;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Server._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoLifeSupportSystem : SharedCryoLifeSupportSystem
{
    /// <inheritdoc/>
    [Dependency] private readonly IGameTiming _time = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;
    [Dependency] private readonly GasAnalyzerSystem _gasAnalyzer = default!;
    [Dependency] private readonly AtmosphereSystem _atmos = default!;
    [Dependency] private readonly GasCanisterSystem _gasCan = default!;
    [Dependency] private readonly HealthAnalyzerSystem _healthAnalyzer = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;
    [Dependency] private readonly NodeContainerSystem _node = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryoLifeSupportComponent, AtmosDeviceUpdateEvent>(OnAtmosDeviceUpdate);
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        var query = EntityQueryEnumerator<CryoLifeSupportComponent>();

        while (query.MoveNext(out var owner, out var comp))
        {
            if (_time.CurTime < comp.NextUiUpdate)
                continue;

            comp.NextUiUpdate =  _time.CurTime + comp.UiUpdateInterval;
            UpdateUi((owner, comp));
        }
    }

    private void OnAtmosDeviceUpdate(Entity<CryoLifeSupportComponent> ent, ref AtmosDeviceUpdateEvent args)
    {
        // it's tempting to make this dependent on being powered,
        // but a blackout would mean a quick death by CO2/N2O poisoning and suffocation.
        if (!_node.TryGetNode<PortablePipeNode>(ent.Owner, "port", out var node) ||
            ent.Comp.CapsuleSlot?.Item is not { } capEnt ||
            !TryComp<CryoCapsuleComponent>(capEnt, out var capComp))
            return;

        _atmos.React(capComp.Air, node);

        if (node.NodeGroup is PipeNet { NodeCount: > 1 } net)
        {
            _gasCan.MixContainerWithPipeNet(capComp.Air, net.Air);
        }
    }

    private void UpdateUi(Entity<CryoLifeSupportComponent> ent)
    {
        if (!_ui.IsUiOpen(ent.Owner, CryoLifeSupportUiKey.Key))
            return;

        var health = _healthAnalyzer.GetHealthAnalyzerUiState(ent.Comp.CapsuleSlot?.Item);
        var gasMix = GetGasInfo(ent);
        var (beakerCapacity, beaker) = GetBeakerInfo(ent);

        _ui.ServerSendUiMessage(
            ent.Owner,
            CryoLifeSupportUiKey.Key,
            new CryoLifeSupportBuiMessage(gasMix, health, beakerCapacity, beaker));
    }


    private (FixedPoint2? capacity, List<ReagentQuantity>? reagents) GetBeakerInfo(Entity<CryoLifeSupportComponent> ent)
    {
        if (ent.Comp.BeakerSlot?.Item is not { } beaker ||
            !_solution.TryGetFitsInDispenser(beaker,
                out var soln,
                out var solution))
            return (null, null);

        return (soln.Value.Comp.Solution.MaxVolume, soln.Value.Comp.Solution.Contents);
    }

    private GasMixEntry? GetGasInfo(Entity<CryoLifeSupportComponent> ent)
    {
        if (ent.Comp.CapsuleSlot?.Item is not { } capEnt ||
            !TryComp<CryoCapsuleComponent>(capEnt, out var capComp))
            return null;

        return _gasAnalyzer.GenerateGasMixEntry("capsule", capComp.Air);
    }
}
