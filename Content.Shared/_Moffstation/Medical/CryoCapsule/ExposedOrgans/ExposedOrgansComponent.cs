using Content.Shared.Body;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.ExposedOrgans;

/// <summary>
/// This is used for body that have internal organs reachable from the outside.
/// </summary>
[RegisterComponent]
public sealed partial class ExposedOrgansComponent : Component
{
    /// <summary>
    /// Organs that can be accessed
    /// </summary>
    [DataField]
    public Dictionary<string, ProtoId<OrganCategoryPrototype>> ExposedOrgans =  new Dictionary<string, ProtoId<OrganCategoryPrototype>>();

    /// <summary>
    /// Exposed organs present inside the body, associated to their category.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<OrganCategoryPrototype>, EntityUid?> PresentOrgans = new Dictionary<ProtoId<OrganCategoryPrototype>, EntityUid?>();

    /// <summary>
    /// Sound that will be played when inserting or removing exposed organs.
    /// </summary>
    [DataField]
    public SoundSpecifier? InteractionSound = new SoundPathSpecifier("/Audio/Voice/Slime/slime_squish.ogg");
}
