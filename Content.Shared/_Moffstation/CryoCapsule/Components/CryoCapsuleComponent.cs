using Content.Shared.Atmos;

namespace Content.Shared._Moffstation.CryoCapsule.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CryoCapsuleComponent : Component
{
    /// <summary>
    /// Identification string for the organs
    /// </summary>
    public const string OrganGroupName = "capsule";

    /// <summary>
    /// Volume of gas (in L) the capsule can contain
    /// </summary>
    [DataField]
    public float AirVolume = 20;

    /// <summary>
    /// Gas contained inside the capsule
    /// </summary>
    [DataField]
    public GasMixture Air = new GasMixture();
}
