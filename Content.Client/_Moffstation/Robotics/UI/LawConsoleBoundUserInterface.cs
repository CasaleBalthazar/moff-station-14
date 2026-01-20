using Content.Shared._Moffstation.Robotics;
using Robust.Client.UserInterface;

namespace Content.Client._Moffstation.Robotics.UI;

public sealed class LawConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    public LawConsoleWindow _window = default!;

    private readonly EntityManager _entityManager;

    private EntityUid _consoleUid;

    public LawConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _window = this.CreateWindow<LawConsoleWindow>();
        _entityManager = IoCManager.Resolve<EntityManager>();
        _consoleUid = owner;
    }

    protected override void Open()
    {
        base.Open();

        _window.OnTransfer += (name, lawset) => SendMessage(new LawConsoleTransferMessage(name, lawset, _entityManager.GetNetEntity(_consoleUid)));
        _window.OnSave += (name, lawset) => SendMessage(new LawConsoleSaveMessage(name, lawset, _entityManager.GetNetEntity(_consoleUid)));
        _window.OnDelete += (name) => SendMessage(new LawConsoleDeleteMessage(name, _entityManager.GetNetEntity(_consoleUid)));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not LawConsoleState cast)
            return;

        _window.UpdateState(cast);
    }
}
