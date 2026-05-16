using Content.Shared._Moffstation.CryoCapsule.Components;
using Robust.Client.UserInterface;

namespace Content.Client._Moffstation.CryoCapsule;

public sealed class CryoLifeSupportBoundUserInterface : BoundUserInterface
{
    private CryoLifeSupportWindow? _window = default!;

    public CryoLifeSupportBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey) { }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<CryoLifeSupportWindow>();
        _window.Title = EntMan.GetComponent<MetaDataComponent>(Owner).EntityName;

        _window.OnEjectBeakerPressed += () => SendMessage(
            new CryoLifeSupportSimpleUiMessage(CryoLifeSupportSimpleUiMessage.MessageType.EjectBeaker));
        _window.OnEjectPatientPressed += () => SendMessage(
            new CryoLifeSupportSimpleUiMessage(CryoLifeSupportSimpleUiMessage.MessageType.EjectCapsule));
        _window.OnRevivePatientPressed += () => SendMessage(
            new CryoLifeSupportSimpleUiMessage(CryoLifeSupportSimpleUiMessage.MessageType.ReviveCapsule));
        // todo : of course change in function of destination
        _window.OnInjectionPressed += amount => SendMessage(
            new CryoLifeSupportInjectionUiMessage(CryoLifeSupportInjectionUiMessage.MessageDestination.Metabolism, amount));
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        if (message is not CryoLifeSupportBuiMessage state)
            return;

        _window?.SetState(state);
    }
}
