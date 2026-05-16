using System.Linq;
using Content.Shared._Moffstation.Body.Components;
using Content.Shared._Moffstation.Body.Events;
using Content.Shared.Body;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Shared._Moffstation.Body.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class DisplayableOrgansSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DisplayableOrgansComponent, ComponentStartup>(OnComponentStartup);

        SubscribeLocalEvent<DisplayableOrgansComponent, OrganInsertedIntoEvent>(OnOrganInsertedInto);
        SubscribeLocalEvent<DisplayableOrgansComponent, OrganRemovedFromEvent>(OnOrganRemovedFrom);
    }

    private void OnComponentStartup(Entity<DisplayableOrgansComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<BodyComponent>(ent, out var body) || body.Organs is null)
            return;

        foreach (var organ in body.Organs.ContainedEntities)
        {
            if (! TryComp<OrganComponent>(organ, out var comp) || comp.Category is not { } category)
                continue;

            ent.Comp.Organs.Add(category, organ);

            if ( !ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Displayed))
                continue;

            var ev = new OrganGotDisplayedEvent(ent.Owner);
            RaiseLocalEvent(organ, ev);
        }
    }

    private void OnOrganInsertedInto(Entity<DisplayableOrgansComponent> ent, ref OrganInsertedIntoEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.Organs.Add(category, ev.Organ);

        if (!ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Displayed))
            return;

        var info = new OrganGotDisplayedEvent(ent.Owner);
        RaiseLocalEvent(ev.Organ, info);
    }

    private void OnOrganRemovedFrom(Entity<DisplayableOrgansComponent> ent, ref OrganRemovedFromEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.Organs.Remove(category);

        if (!ent.Comp.OrganGroups.Values.Any(group => group.Categories.Contains(category) && group.Displayed))
            return;

        var info = new OrganGotHiddenEvent(ent.Owner);
        RaiseLocalEvent(ev.Organ, info);
    }


    # region public API

    /// <summary>
    /// Attempt to display an organ group inside the body.
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public bool TryDisplay(Entity<DisplayableOrgansComponent?> ent, string group)
    {
        if (!Resolve(ent.Owner, ref ent.Comp) || ! ent.Comp.OrganGroups.TryGetValue(group, out var organs))
            return false;

        if (organs.Displayed)
            return true;

        foreach (var category in organs.Categories)
        {
            if (! ent.Comp.Organs.TryGetValue(category, out var toDisplay))
                continue;

            var ev = new OrganGotDisplayedEvent(ent.Owner);
            RaiseLocalEvent(toDisplay, ev);
        }

        organs.Displayed = true;
        return true;
    }

    /// <summary>
    /// Attempt to hide an organ group inside the body.
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public bool TryHide(Entity<DisplayableOrgansComponent?> ent, string group)
    {
        if (!Resolve(ent.Owner, ref ent.Comp) || ! ent.Comp.OrganGroups.TryGetValue(group, out var organs))
            return false;

        if (! organs.Displayed)
            return true;

        foreach (var category in organs.Categories)
        {
            if (! ent.Comp.Organs.TryGetValue(category, out var toDisplay))
                continue;

            var ev = new OrganGotHiddenEvent(ent.Owner);
            RaiseLocalEvent(toDisplay, ev);
        }

        organs.Displayed = false;
        return true;
    }

    #endregion
}
