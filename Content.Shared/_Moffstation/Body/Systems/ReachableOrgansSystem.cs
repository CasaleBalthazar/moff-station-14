using System.Linq;
using Content.Shared._Moffstation.Body.Components;
using Content.Shared._Moffstation.Body.Events;
using Content.Shared.Body;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Body.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class ReachableOrgansSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ReachableOrgansComponent, ComponentStartup>(OnComponentStartup);

        SubscribeLocalEvent<ReachableOrgansComponent, OrganRemovedFromEvent>(OnOrganRemovedFrom);
        SubscribeLocalEvent<ReachableOrgansComponent, OrganInsertedIntoEvent>(OnOrganInsertedInto);

        SubscribeLocalEvent<ReachableOrgansComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<ReachableOrgansComponent, GetVerbsEvent<AlternativeVerb>>(OnGetAlternativeVerb);
    }

    private void OnComponentStartup(Entity<ReachableOrgansComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<BodyComponent>(ent, out var body) || body.Organs is null)
            return;

        foreach (var organ in body.Organs.ContainedEntities)
        {
            if (! TryComp<OrganComponent>(organ, out var comp) || comp.Category is not { } category)
                continue;

            ent.Comp.Organs.Add(category, organ);

            if ( !ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Exposed))
                continue;

            var ev = new OrganGotExposedEvent(ent.Owner);
            RaiseLocalEvent(organ, ev);
        }

    }

    private void OnOrganInsertedInto(Entity<ReachableOrgansComponent> ent, ref OrganInsertedIntoEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.Organs.Add(category, ev.Organ);

        if (!ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Exposed))
            return;

        var info = new OrganGotExposedEvent(ent.Owner);
        RaiseLocalEvent(ev.Organ, info);
    }

    private void OnOrganRemovedFrom(Entity<ReachableOrgansComponent> ent, ref OrganRemovedFromEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.Organs.Remove(category);

        if (!ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Exposed))
            return;

        var info = new OrganGotUnexposedEvent(ent.Owner);
        RaiseLocalEvent(ev.Organ, info);
    }

    private void OnGetAlternativeVerb(Entity<ReachableOrgansComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        var user = args.User;

        foreach (var (category, organ) in ent.Comp.Organs)
        {
            if (! ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Exposed))
                continue;

            args.Verbs.Add(new AlternativeVerb
            {
                IconEntity = GetNetEntity(organ),
                Text = "name", // todo !
                Category = VerbCategory.Eject,
                Act = () => _audio.PlayPredicted(ent.Comp.EjectSound, ent, user), // todo !
            });
        }
    }

    private void OnInteractUsing(Entity<ReachableOrgansComponent> ent, ref InteractUsingEvent args)
    {
        if (!TryComp<OrganComponent>(args.Used, out var organ) ||
            organ.Category is not { } category ||
            !TryComp<BodyComponent>(ent, out var body) ||
            body.Organs is null)
            return;

        args.Handled = true;
        if (!ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Exposed))
            return;

        if (ent.Comp.Organs.TryGetValue(category, out var present))
        {
            _container.Remove(present, body.Organs);
            _container.Insert(args.Used, body.Organs);
            _hands.PickupOrDrop(args.User, present);
        }
        else
        {
            _container.Insert(args.Used, body.Organs);
        }
        _audio.PlayPredicted(ent.Comp.InsertSound, ent, args.User);
    }

    # region public API

    public bool TryExpose(Entity<ReachableOrgansComponent?> ent, string group)
    {
        if (!Resolve(ent.Owner, ref ent.Comp) || ! ent.Comp.OrganGroups.TryGetValue(group, out var organs))
            return false;

        if (organs.Exposed)
            return true;

        foreach (var category in organs.Categories)
        {
            if (! ent.Comp.Organs.TryGetValue(category, out var toDisplay))
                continue;

            var ev = new OrganGotExposedEvent(ent.Owner);
            RaiseLocalEvent(toDisplay, ev);
        }

        organs.Exposed = true;
        return true;

        return true;
    }

    public bool TryUnexpose(Entity<ReachableOrgansComponent?> ent, string group)
    {
        if (!Resolve(ent.Owner, ref ent.Comp) || ! ent.Comp.OrganGroups.TryGetValue(group, out var organs))
            return false;

        if (! organs.Exposed)
            return true;

        foreach (var category in organs.Categories)
        {
            if (! ent.Comp.Organs.TryGetValue(category, out var toDisplay))
                continue;

            var ev = new OrganGotUnexposedEvent(ent.Owner);
            RaiseLocalEvent(toDisplay, ev);
        }

        organs.Exposed = false;
        return true;
    }

    # endregion

}
