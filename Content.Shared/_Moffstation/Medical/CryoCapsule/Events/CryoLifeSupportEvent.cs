namespace Content.Shared._Moffstation.Medical.CryoCapsule.Events;


/// <summary>
/// Raised on the entity inside the machine to know if they can be revived.
/// </summary>
[ByRefEvent]
public sealed class CryoLifeSupportQueryStatusEvent : EntityEventArgs
{
    public enum Status { Alive, Ready, Unready}

    public Status ReviveStatus = Status.Unready;
    public List<string> Reasons = new();
}

/// <summary>
/// Raised on the entity inside the machine
/// </summary>
public sealed class CryoLifeSupportReviveAttemptEvent : EntityEventArgs
{

}
