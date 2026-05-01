using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.CryoCapsule;

/// <summary>
/// This is used for items encasing an organism inside a mini-cryopod.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class CryoCapsuleComponent : Component
{
    /// <summary>
    /// The name of the container the organism is stored in.
    /// </summary>
    public const string BodyContainerName = "body_container";

    /// <summary>
    /// If true, the mob inside cannot be accessed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool BodyAccessible;

    /// <summary>
    /// Container which contain the mob inside.
    /// </summary>
    [ViewVariables]
    public ContainerSlot BodyContainer = default!;

}

// i THINK it's how you should do ?
[Serializable, NetSerializable]
public enum CryoCapsuleMobVisuals : byte
{
    Base,
    Brain,
    Eyes,
    Stomach,
    Lungs,
    Heart,
}
