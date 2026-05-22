using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// Set player speed to zero and standing state to down, simulating leg paralysis.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LegsParalyzedSystem))]
public sealed partial class LegsParalyzedComponent : Component
{
    // Moffstation - Begin

    /// <summary>
    /// walk speed of the entity when legs are not paralyzed
    /// </summary>
    [DataField]
    public float WalkSpeed = 0f;

    /// <summary>
    /// sprint speed of the entity when legs are not paralyzed
    /// </summary>
    [DataField]
    public float SprintSpeed = 0f;

    /// <summary>
    /// acceleration of the entity when legs are not paralyzed
    /// </summary>
    [DataField]
    public float Acceleration = 0f;

    // Moffstation - End
}
