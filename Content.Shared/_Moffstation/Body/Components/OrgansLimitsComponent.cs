using Content.Shared.Body;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Body.Components;

/// <summary>
/// This is used in correlation with <see cref="BodyComponent"/> to ensure the number of organs of a certain
/// type inside a body do not go beyond a certain limit.
/// </summary>
[RegisterComponent]
public sealed partial class OrgansLimitsComponent : Component
{
    /// <summary>
    /// Authorised amount of organs, by organ category
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<OrganCategoryPrototype>, int> Limits;
}
