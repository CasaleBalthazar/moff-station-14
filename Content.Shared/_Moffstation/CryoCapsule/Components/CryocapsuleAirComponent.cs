using Content.Shared.Atmos;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CryocapsuleAirComponent : Component
{
    /// <summary>
    /// Volume of gas contained inside the capsule
    /// </summary>
    [DataField]
    public float Volume = 20f;

    /// <summary>
    /// Gas mixture contained inside the capsule
    /// </summary>
    [DataField]
    public GasMixture Air = new GasMixture();
}
