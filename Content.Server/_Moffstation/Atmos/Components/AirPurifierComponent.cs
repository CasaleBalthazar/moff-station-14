using Content.Shared.Atmos;

namespace Content.Server._Moffstation.Atmos.Components;

/// <summary>
/// This is used for entity that filter gases from a container and release them in the environment.
/// Work with <see cref="AtmosDeviceComponent"/>
/// </summary>
/// <remarks>
/// I know AirFilterComponent would be a better name, but it's already taken <see cref="AirFilterComponent"/>
/// </remarks>
[RegisterComponent]
public sealed partial class AirPurifierComponent : Component
{
    /// <summary>
    /// Gases that will be filtered out
    /// </summary>
    [DataField]
    public List<Gas> FilteredGases = [Gas.NitrousOxide, Gas.CarbonDioxide];

    /// <summary>
    /// Gas mixture the entity will filter from
    /// </summary>
    [DataField]
    public GasMixture? Air;
}
