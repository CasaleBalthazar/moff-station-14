using Content.Shared._Moffstation.CryoCapsule.Components;
using Content.Shared.Audio;
using Content.Shared.Body.Events;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Damage.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Stacks;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.CryoCapsule.Systems;

/// <summary>
/// This handles...
/// </summary>
public abstract class SharedCryoLifeSupportSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _ambientSound = default!;
    [Dependency] private readonly SharedPointLightSystem _ambientLight = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutions = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CryoLifeSupportComponent, ComponentInit>(OnComponentInit);

        SubscribeLocalEvent<CryoLifeSupportComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
        SubscribeLocalEvent<CryoLifeSupportComponent, EntRemovedFromContainerMessage>(OnEntRemoved);
        SubscribeLocalEvent<CryoLifeSupportComponent, PowerChangedEvent>(OnPowerChanged);

        // activate all metabolic stage even when dead
        SubscribeLocalEvent<InsideCryoLifeSupportComponent, GetMetabolicStageDeadEvent>(OnGetMetabolicStageDead);

        Subs.BuiEvents<CryoLifeSupportComponent>(CryoLifeSupportUiKey.Key, subs =>
            {
                subs.Event<CryoLifeSupportSimpleUiMessage>(OnSimpleUiMessage);
                subs.Event<CryoLifeSupportInjectionUiMessage>(OnInjectionUiMessage);
            });
    }

    private void OnComponentInit(Entity<CryoLifeSupportComponent> ent, ref ComponentInit args)
    {
        if (_slots.TryGetSlot(ent, "lifesupport-capsule", out var capSlot))
            ent.Comp.CapsuleSlot = capSlot;

        if (_slots.TryGetSlot(ent, "lifesupport-solution", out var solSlot))
            ent.Comp.BeakerSlot = solSlot;
    }

    private void OnEntInserted(Entity<CryoLifeSupportComponent> ent, ref EntInsertedIntoContainerMessage msg)
    {
        if (msg.Container.ID != ent.Comp.CapsuleSlot.ID ||
            ! _power.IsPowered(ent.Owner))
            return;

        _ambientSound.SetAmbience(ent, true);
        _ambientLight.SetEnabled(ent, true);

        AddComp<InsideCryoLifeSupportComponent>(msg.Entity);
    }

    private void OnEntRemoved(Entity<CryoLifeSupportComponent> ent, ref EntRemovedFromContainerMessage msg)
    {
        if (msg.Container.ID != ent.Comp.CapsuleSlot.ID ||
            ! _power.IsPowered(ent.Owner))
            return;

        _ambientSound.SetAmbience(ent, false);
        _ambientLight.SetEnabled(ent, false);

        RemComp<InsideCryoLifeSupportComponent>(msg.Entity);
    }

    private void OnPowerChanged(Entity<CryoLifeSupportComponent> ent, ref PowerChangedEvent ev)
    {
        var active = ev.Powered && ent.Comp.CapsuleSlot.Item is not null;
        _ambientSound.SetAmbience(ent, active);
        _ambientLight.SetEnabled(ent, active);

        if (ent.Comp.CapsuleSlot.Item is not { } patient)
            return;

        if (ev.Powered)
            AddComp<InsideCryoLifeSupportComponent>(patient);
        else
            RemComp<InsideCryoLifeSupportComponent>(patient);
    }

    private void OnGetMetabolicStageDead(Entity<InsideCryoLifeSupportComponent> ent, ref GetMetabolicStageDeadEvent ev)
    {
        ev.Active = true;
    }

    private void OnSimpleUiMessage(Entity<CryoLifeSupportComponent> ent, ref CryoLifeSupportSimpleUiMessage msg)
    {
        switch (msg.Type)
        {
            case CryoLifeSupportSimpleUiMessage.MessageType.EjectCapsule :
                _slots.TryEjectToHands(ent, ent.Comp.CapsuleSlot, msg.Actor);
                break;
            case CryoLifeSupportSimpleUiMessage.MessageType.EjectBeaker :
                _slots.TryEjectToHands(ent, ent.Comp.BeakerSlot, msg.Actor);
                break;
            case CryoLifeSupportSimpleUiMessage.MessageType.ReviveCapsule :
                TryZap(ent);
                break;
            default :
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnInjectionUiMessage(Entity<CryoLifeSupportComponent> ent, ref CryoLifeSupportInjectionUiMessage msg)
    {
        if (TryInject(ent, msg.Amount))
            _audio.PlayPredicted(ent.Comp.InjectSound, ent, msg.Actor);
    }

    # region private methods

    private bool CanZap(Entity<CryoLifeSupportComponent> ent)
    {
        if (ent.Comp.CapsuleSlot.Item is not { } patient ||
            ! _power.IsPowered(ent.Owner) ||
            ! TryComp<MobStateComponent>(patient, out var mobState) ||
            ! _mobState.IsDead(patient, mobState))
            return false;

        return true;
    }

    private bool TryZap(Entity<CryoLifeSupportComponent> ent)
    {
        if (!CanZap(ent))
            return false;

        if (ent.Comp.CapsuleSlot.Item is not { } patient)
            return false;

        if (TryComp<MobThresholdsComponent>(patient, out var thresholds) &&
            _mobThreshold.TryGetThresholdForState(patient, MobState.Dead, out var threshold, thresholds) &&
            _damageable.GetTotalDamage(patient) < threshold)
        {
            _mobState.ChangeMobState(patient, MobState.Critical);
        }

        _audio.PlayPredicted(ent.Comp.ZapSound, ent, ent);

        return true;
    }


    private bool TryInject(Entity<CryoLifeSupportComponent> ent, FixedPoint2 amount)
    {
        if (ent.Comp.BeakerSlot.Item is not { } beaker ||
            ! _solutions.TryGetFitsInDispenser(beaker, out var beakerSol, out var bSol) ||
            ent.Comp.CapsuleSlot.Item is not { } capsule ||
            ! _solutions.TryGetInjectableSolution(capsule, out var targetSol, out var tSol))
            return false;

        var transferedAmount = FixedPoint2.Min(tSol.AvailableVolume, FixedPoint2.Min(bSol.Volume, amount));

        if (transferedAmount <= FixedPoint2.Zero)
            return false;

        Solution removedSolution;
        if (TryComp<StackComponent>(capsule, out var stack))
            removedSolution = _solutions.SplitStackSolution(beakerSol.Value, transferedAmount, stack.Count);
        else
            removedSolution = _solutions.SplitSolution(beakerSol.Value, transferedAmount);

        _solutions.Inject(capsule, targetSol.Value, removedSolution);

        return true;
    }

    # endregion
}
