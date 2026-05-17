using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Systems;
using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared._Moffstation.CryoCapsule.Systems;

namespace Content.Server._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class CryoCapsuleSystem : SharedCryoCapsuleSystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryoCapsuleComponent, InhaleLocationEvent>(OnInhaleLocation);
        SubscribeLocalEvent<CryoCapsuleComponent, ExhaleLocationEvent>(OnExhaleLocation);
        SubscribeLocalEvent<CryoCapsuleComponent, GasAnalyzerScanEvent>(OnGasAnalyzerScan);
    }

    private void OnInhaleLocation(Entity<CryoCapsuleComponent> ent, ref InhaleLocationEvent ev)
    {
        ev.Gas = ent.Comp.Air;
    }

    private void OnExhaleLocation(Entity<CryoCapsuleComponent> ent, ref ExhaleLocationEvent ev)
    {
        ev.Gas = ent.Comp.Air;
    }

    private void OnGasAnalyzerScan(Entity<CryoCapsuleComponent> ent, ref GasAnalyzerScanEvent ev)
    {
        ev.GasMixtures ??= [];
        ev.GasMixtures.Add(("capsule", ent.Comp.Air));
    }
}
