namespace Content.Shared._Moffstation.Medical.CryoCapsule.HeldTogether;

/// <summary>
/// This is used for entities that can only exist inside of container.
/// Entity is gibbed instantly when removed.
/// </summary>
[RegisterComponent]
public sealed partial class GibWhenOutsideContainerComponent : Component
{

}
