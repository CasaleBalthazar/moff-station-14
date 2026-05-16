using Robust.Shared.Serialization;

namespace Content.Shared._Moffstation.Humanoid;

[Serializable, NetSerializable]
public enum HumanoidExposableLayers : byte
{
    Brain,
    Eyes,
    Ears,
    Tongue,
    Liver,
    Stomach,
    Kidneys,
    Appendix,
    Lungs,
    Heart,
}
