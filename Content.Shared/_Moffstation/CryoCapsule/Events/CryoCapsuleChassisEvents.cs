namespace Content.Shared._Moffstation.CryoCapsule.Events;

public class ChassisInsertAttemptEvent : CancellableEntityEventArgs
{
    public string? CancelReason;
}
